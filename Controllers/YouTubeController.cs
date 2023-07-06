using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YouTupe.Controllers
{

    [ApiController]
    [Route("api/videos")]
    public class YouTubeController : ControllerBase
    {
        [HttpPost]
        public IActionResult DownloadVideo([FromBody] VideoLinkRequest request)
        {
            string youtubeLink = request.YoutubeLink;
            string fileName = GenerateFileName();

            if (!string.IsNullOrEmpty(youtubeLink))
            {
                try
                {
                    string videoUrl = GetVideoUrl(youtubeLink);
                    DownloadVideoFile(videoUrl, fileName);
                    return Ok(new { Message = "Video downloaded successfully." });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = $"Error occurred while downloading the video: {ex.Message}" });
                }
            }
            else
            {
                return BadRequest(new { Message = "Please provide a valid YouTube link." });
            }
        }

        private string GetVideoUrl(string youtubeLink)
        {
            // Extract the video ID from the YouTube link
            string videoId = youtubeLink.Substring(youtubeLink.IndexOf("v=") + 2);
         //   videoId = videoId.Substring(0, videoId.IndexOf("&"));

            // Construct the download URL for the video
            string videoUrl = "https://www.youtube.com/watch?v=" + videoId;

            return videoUrl;
        }

        private void DownloadVideoFile(string videoUrl, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(videoUrl, Path.Combine("C:\\youtube", fileName));
            }
        }

        private string GenerateFileName()
        {
            // Generate a unique file name for the downloaded video
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";
            return fileName;
        }
    }

    public class VideoLinkRequest
    {
        public string YoutubeLink { get; set; }
    }
}
