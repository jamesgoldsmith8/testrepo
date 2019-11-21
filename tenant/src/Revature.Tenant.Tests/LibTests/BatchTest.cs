using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Revature.Tenant.Tests.LibTests
{
  public class BatchTest
  {
    /// <summary>
    /// Tests that Empty ID throw exception
    /// </summary>
    [Fact]
    public void Batch_Id_Test()
    {
      Assert.ThrowsAny<ArgumentException>(() => new Lib.Models.Batch { Id = -1 });
    }

    /// <summary>
    /// Tests that Empty language throw exception
    /// </summary>
    [Fact]
    public void Batch_Language_Empty()
    {
      Assert.ThrowsAny<ArgumentException>(() => new Lib.Models.Batch { BatchLanguage = "" });
    }

    /// <summary>
    /// Tests that Empty train center ID throw exception
    /// </summary>
    [Fact]
    public void Batch_Training_Center_Empty()
    {
      Assert.ThrowsAny<ArgumentException>(() => new Lib.Models.Batch{ TrainingCenter = Guid.Empty });
    }

  }
}
