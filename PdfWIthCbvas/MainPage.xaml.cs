using System;
using System.IO;
using PdfiumViewer;
using PDFtoImage;

namespace PdfWIthCbvas;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        //	if (count == 1)
        //CounterBtn.Text = $"Clicked {count} time";
        //else
        //	CounterBtn.Text = $"Clicked {count} times";

        //	SemanticScreenReader.Announce(CounterBtn.Text);

    }
    private async void aaab(object sender, EventArgs e)

    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Pdf,
            PickerTitle = "Select a PDF file"
        });
     
        if (result != null)
        {
            var stream = new MemoryStream();
            var pdfDocument = PdfDocument.Load(result.FullPath);
            var pdf = new PdfViewer();
          //  pdf = pdfDocument;
            for (int page = 0; page < pdfDocument.PageCount; page++)
            {          // Loop through pages
                using (var img = pdfDocument.Render(page, 300, 300, false))
                {   // Render with dpi and with forPrinting false
                    //   img.Save(@"image.bmp", ImageFormat.Bmp);     // Save rendered image to disc
                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Position = 0;
                    var imageSource = new StreamImageSource
                    {
                        Stream = (token) => Task.FromResult((Stream)stream)
                    };

                    imgpdf.Source = imageSource;

                }
            }


            if (pdfDocument != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    pdfDocument.Save(memoryStream);
                    byte[] pdfBytes = memoryStream.ToArray();
                    webView.Source = new HtmlWebViewSource
                    {
                        Html = $"<html><body><embed type='application/pdf' src='data:application/pdf;base64,{Convert.ToBase64String(pdfBytes)}' width='100%' height='100%'></body></html>"
                    };
                }
            }
            else
            {
                // Show an error message indicating that the file is not a valid PDF file
            }


            //  webView.Source = new HtmlWebViewSource
            // {
            //     Html = $"<html><body><embed type='application/pdf' src='data:application/pdf;base64,{Convert.ToBase64String(pdfDocument)}' width='100%' height='100%'></body></html>"
            //  };
        }

    }
    


}







