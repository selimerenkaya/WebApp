using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public class GenderPredictor
{
    private readonly InferenceSession _session;

    public GenderPredictor(string modelPath)
    {
        _session = new InferenceSession(modelPath);
    }

    public string PredictGender(string imagePath)
    {
        using var image = Image.Load<Rgb24>(imagePath);
        image.Mutate(x => x.Resize(64, 64));

        var input = new DenseTensor<float>(new[] { 1, 3, 64, 64 });

        for (int y = 0; y < 64; y++)
        {
            for (int x = 0; x < 64; x++)
            {
                var pixel = image[x, y];
                input[0, 0, y, x] = pixel.R / 255f;
                input[0, 1, y, x] = pixel.G / 255f;
                input[0, 2, y, x] = pixel.B / 255f;
            }
        }

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input", input)
        };

        using var results = _session.Run(inputs);
        var output = results.First().AsEnumerable<float>().ToArray();

        return output[0] > output[1] ? "Erkek" : "Kadın";
    }
}
