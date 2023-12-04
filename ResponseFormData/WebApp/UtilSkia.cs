using SkiaSharp;

namespace WebApp
{
    public static class UtilSkia
    {
        public static Stream OpenGrayscale(Stream stream)
        {
            using (var originalBitmap = SKBitmap.Decode(stream))
            {
                using (var grayscaleBitmap = ConvertToGrayscale(originalBitmap))
                {
                    var ms = new MemoryStream();
                    grayscaleBitmap.Encode(ms, SKEncodedImageFormat.Png, 100);
                    ms.Position = 0;
                    return ms;
                }
            }
        }

        private static SKBitmap ConvertToGrayscale(SKBitmap originalBitmap)
        {
            float[] values = new float[]
               {
                0.2125f, 0.7154f, 0.0721f, 0, 0,
                0.2125f, 0.7154f, 0.0721f, 0, 0,
                0.2125f, 0.7154f, 0.0721f, 0, 0,
                0, 0, 0, 1, 0
               };

            // 新しいSKBitmapを作成して、グレースケールに変換する
            var grayscaleBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height);

            using (var canvas = new SKCanvas(grayscaleBitmap))
            using (var paint = new SKPaint())
            {
                // グレースケールに変換するためのフィルタを設定
                var colorFilter = SKImageFilter.CreateColorFilter(SKColorFilter.CreateColorMatrix(values));

                // ペイントにフィルタを設定
                paint.ImageFilter = colorFilter;

                // グレースケールに変換してcanvasに描画
                canvas.DrawBitmap(originalBitmap, 0, 0, paint);
            }

            return grayscaleBitmap;
        }
    }
}
