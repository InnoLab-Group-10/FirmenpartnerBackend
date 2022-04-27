using AutoMapper;
using FirmenpartnerBackend.Configuration;
using FirmenpartnerBackend.Data;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/file")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FileController : ControllerBase
    {
        protected ApiDbContext dbContext;
        protected IMapper mapper;
        protected FileUploadConfig fileUploadConfig;

        public FileController(ApiDbContext dbContext, IMapper mapper, FileUploadConfig fileUploadConfig)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.fileUploadConfig = fileUploadConfig;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetAll()
        {
            string user = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;

            List<FileEntry> models = dbContext.FileEntries.Where(f => f.OwnerId == user).ToList();

            List<FileBaseResponse> results = models.Select(e => mapper.Map<FileBaseResponse>(e)).ToList();
            return Ok(new FileMultiResponse()
            {
                Success = true,
                Results = results
            });
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                FileEntry? model = await dbContext.FileEntries.FindAsync(id);

                if (model == null)
                {
                    return NotFound(new FileSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No file with the given ID exists." }
                    });
                }
                else
                {
                    string filePath = Path.Combine(fileUploadConfig.TargetPath, model.Id.ToString());

                    if (System.IO.File.Exists(filePath))
                    {
                        return PhysicalFile(Path.GetFullPath(filePath), "application/octet-stream", fileDownloadName: model.Name);
                    }
                    else
                    {
                        return NotFound(new FileSingleResponse()
                        {
                            Success = false,
                            Errors = new List<string>() { "No file with the given ID exists." }
                        });
                    }
                }

            }

            return BadRequest(new FileSingleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file == null || file.Length > fileUploadConfig.MaxSize) return BadRequest(new FileSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { $"Invalid file size. Uploaded files can be at most {fileUploadConfig.MaxSize} bytes." }
                });

                // Add DB entry
                FileEntry model = new FileEntry()
                {
                    Id = Guid.NewGuid(),
                    Name = file.FileName,
                    Size = file.Length,
                    Timestamp = DateTime.Now,
                    OwnerId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value
                };

                await dbContext.FileEntries.AddAsync(model);
                await dbContext.SaveChangesAsync();

                // Copy actual file
                string filePath = Path.Combine(fileUploadConfig.TargetPath, model.Id.ToString());
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return Ok(new FileSingleResponse()
                {
                    Success = true,
                    Id = model.Id
                });
            }

            return BadRequest(new FileSingleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                FileEntry? model = await dbContext.FileEntries.FindAsync(id);

                string user = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;

                if (model == null || model.OwnerId != user)
                {
                    return NotFound(new DeleteResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No file belonging to the current user with the given ID exists." }
                    });
                }
                else
                {
                    dbContext.FileEntries.Remove(model);
                    await dbContext.SaveChangesAsync();

                    string filePath = Path.Combine(fileUploadConfig.TargetPath, model.Id.ToString());
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    return Ok(new DeleteResponse()
                    {
                        Success = true
                    });
                }

            }

            return BadRequest(new DeleteResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }
    }
}
