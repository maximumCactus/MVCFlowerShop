using Microsoft.AspNetCore.Mvc;
using Amazon; //for linking your AWS account
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration; //appsettings.json section
using System.IO; // input output
using Microsoft.AspNetCore.Http;

namespace MVCFlowerShop.Controllers
{
    public class ImageUploadController : Controller
    {
        private const string s3BucketName = "mvcflowershoplab3tp055396";

        // Func 1: Learn how to get the keys from the appsettings json file
        private List<string> getKeys() {
            List<string> keys = new List<string>();

            var builder = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");   // you can also specify other json file here in the future

            IConfiguration configure = builder.Build();
            keys.Add(configure["Values:Key1"]);
            keys.Add(configure["Values:Key2"]);
            keys.Add(configure["Values:Key3"]);

            return keys;
        }

        // Func2: Modify to become upload file page
        public IActionResult Index()
        {

            return View();
        }

        // Func3: Upload Multiple / Single File(s) to S3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessUploadImage(List<IFormFile> imagefile) {
            // 1 - Add on credentials to attach to the account
            List<string> keys = getKeys();

            // 2- Setup an agent to help communicate with s3
            AmazonS3Client s3agent = new AmazonS3Client(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
            
            // 3 - Validate file
            foreach (var singleimage in imagefile) {
                // Is empty file
                if (singleimage.Length <= 0)
                {
                    return BadRequest("File of " + singleimage.FileName + " is no content");
                }
                // Is more than 5MB
                else if (singleimage.Length >= 5242730)
                {
                    return BadRequest("File of " + singleimage.FileName + " is more than 5MB! Please try again!");
                }
                else if (singleimage.ContentType.ToLower() != "image/png" && singleimage.ContentType.ToLower() != "image/jpg" && singleimage.ContentType.ToLower() != "image/jpeg") {
                    return BadRequest("File of " + singleimage.FileName + " is not a valid image file");
                }
                // Pass the validation -> submit to s3
                try
                {
                    PutObjectRequest uploadRequest = new PutObjectRequest
                    {
                        InputStream = singleimage.OpenReadStream(),
                        BucketName = s3BucketName,
                        Key = "images/" + singleimage.FileName,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    await s3agent.PutObjectAsync(uploadRequest);

                }
                catch (AmazonS3Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex) {
                    return BadRequest(ex.Message);
                }
            }

            // Once done uploading, go back to the index page
            return RedirectToAction("Index", "ImageUpload");
        }
    }
}
