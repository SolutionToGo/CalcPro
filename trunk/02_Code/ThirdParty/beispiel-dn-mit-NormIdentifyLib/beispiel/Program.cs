using dcm.Trinity.Definition;
using dcm.Trinity.SDK;
using System;

namespace beispiel
{
  static class Program
  {
    public static void Main(string[] args)
    {
      var sourceFile = "DATANORM.001";
      var resultFile = "result.xml";

      var adapterType = InferAdapterFromFile(sourceFile);

      var factory = new AdapterFactory(@"T:\Gemeinsame Dateien\trinity6");

      var adapter = factory.CreateAdapter(adapterType);

      AuthorizeAdapter(adapter);

      AttachEventHandlers(adapter);

      try
      {
        adapter.Run(sourceFile, resultFile);
        Console.WriteLine("Conversion completed.");
      }
      catch (AdapterFailureException ex)
      {
        Console.WriteLine("Conversion failed with error: " + ex.Message);
      }
    }

    private static TrinityAdapter InferAdapterFromFile(string path)
    {
      var normIdentifier = new dcm.NormIdentifyLib.NormIdentifier();

      var normInfo = normIdentifier.IdentifyNorm(path);

      if (normInfo.Norm != dcm.NormIdentifyLib.Norm.DATANORM)
        throw new ArgumentException("Not a DATANORM file");

      switch (normInfo.Version)
      {
      case "3": return TrinityAdapter.ImportDN3;
      case "4": return TrinityAdapter.ImportDN4;
      case "5": return TrinityAdapter.ImportDN5;
      default: throw new InvalidOperationException("Not a known DATANORM version");
      }
    }

    private static void AuthorizeAdapter(Adapter adapter)
    {
      var authorizer = new AdapterAuthorizer("betasystems+fz/TD22vq%X:VMbb58W@");
      authorizer.Authorize(adapter);
    }

    private static void AttachEventHandlers(Adapter adapter)
    {
      adapter.ProgressChanged += (sender, e) =>
        Console.WriteLine("{0} of {1}", e.Current, e.Maximum);

      adapter.UserLog.LineWritten += (sender, e) =>
        Console.WriteLine("(at {0}): {1}", e.Timestamp, e.Text);
    }
  }
}
