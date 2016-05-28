using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Test.Unit.Service
{
    [TestClass]
    public class LookupCacheTests
    {
        Mock<IDatabaseFactory> _db;
        Mock<MacheteContext> _ctxt;
        Mock<DbSet<Lookup>> _set;
        //Mock<DbQuery<Lookup>> _q;
        LookupCache _serv;
        public LookupCacheTests()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // https://msdn.microsoft.com/en-us/data/dn314429.aspx
            // Testing with a mocking framework (EF6 onwards)
            _db = new Mock<IDatabaseFactory>();
            _ctxt = new Mock<MacheteContext>();
            var list = new List<Lookup> {
                new Lookup { category = LCategory.memberstatus, key = LMemberStatus.Active },
                new Lookup { category = LCategory.memberstatus, key = LMemberStatus.Sanctioned },
                new Lookup { category = LCategory.memberstatus, key = LMemberStatus.Expelled },
                new Lookup { category = LCategory.memberstatus, key = LMemberStatus.Expired },
                new Lookup { category = LCategory.memberstatus, key = LMemberStatus.Inactive },
                new Lookup { category = LCategory.orderstatus, key = LOrderStatus.Active },
                new Lookup { category = LCategory.orderstatus, key = LOrderStatus.Pending },
                new Lookup { category = LCategory.orderstatus, key = LOrderStatus.Completed },
                new Lookup { category = LCategory.orderstatus, key = LOrderStatus.Cancelled },
                new Lookup { category = LCategory.orderstatus, key = LOrderStatus.Expired },
                new Lookup { category = LCategory.emailstatus, key = LEmailStatus.ReadyToSend },
                new Lookup { category = LCategory.emailstatus, key = LEmailStatus.Sent },
                new Lookup { category = LCategory.emailstatus, key = LEmailStatus.Sending },
                new Lookup { category = LCategory.emailstatus, key = LEmailStatus.Pending },
                new Lookup { category = LCategory.emailstatus, key = LEmailStatus.TransmitError }
            }.AsQueryable();
            _set = new Mock<DbSet<Lookup>>();
            _set.As<IQueryable<Lookup>>().Setup(m => m.Provider).Returns(list.Provider);
            _set.As<IQueryable<Lookup>>().Setup(m => m.Expression).Returns(list.Expression);
            _set.As<IQueryable<Lookup>>().Setup(m => m.ElementType).Returns(list.ElementType);
            _set.As<IQueryable<Lookup>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());
            _set.Setup(r => r.AsNoTracking()).Returns(_set.Object);
            _ctxt.Setup(r => r.Lookups).Returns(_set.Object);
            _db.Setup(r => r.Get()).Returns(_ctxt.Object);
            _serv = new LookupCache(_db.Object);

        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Lookups)]
        public void GetCache_returns_Enumerable()
        {
            // ARrange
            // Act
            var result = _serv.getCache();
            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Lookup>));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Lookups)]
        public void getByKeys_returns_int()
        {
            // ARrange
            // Act
            var result = _serv.getByKeys(LCategory.memberstatus, LMemberStatus.Active);
            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [ExpectedException(typeof(MacheteIntegrityException))]
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Lookups)]
        public void getByKeys_throws_exception()
        {
            // ARrange
            // Act
            var result = _serv.getByKeys(LCategory.memberstatus, "invalid");
            // Assert
        }
    }

}
