using Alcoholic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Alcoholic.Controllers.API
{
    [Route("api/Validation/[action]")]
    [ApiController]
    public class CodeValidationController : ControllerBase
    {
        private readonly CodeValidatorService codeValidation;

        public CodeValidationController(CodeValidatorService codeValidation)
        {
            this.codeValidation = codeValidation;
        }

        [HttpGet]
        public IActionResult Generate()
        {
            string code = codeValidation.Generate();
            MemoryStream ms = new();
            Bitmap map = new(100, 40);
            Graphics g = Graphics.FromImage(map);
            g.Clear(Color.Transparent);
            g.DrawString(code, new Font("黑體", 18.0F), Brushes.Gold, new Point(10, 8));
            PaintInterLine(g, 10, map.Width, map.Height);
            map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            // usigned byte => 0~255;
            // signed bytr => -128~127
            byte[] data = ms.GetBuffer();
            return File(data, "image/jpeg");
        }

        [HttpPost]
        public IActionResult Validate([FromBody]string code)
        {
            bool isValid = codeValidation.Validate(code);
            return Ok(isValid);
        }

        private void PaintInterLine(Graphics g, int n, int w, int h)
        {
            Random random = new();
            int startX, startY, endX, endY;
            for (int i = 0; i< n; i++)
            {
                startX = random.Next(0, w);
                startY = random.Next(0, h);
                endX = random.Next(0, w);
                endY = random.Next(0, h);
                g.DrawLine(new Pen(Brushes.Red), startX, startY, endX, endY);
            }
        }
    }
}
