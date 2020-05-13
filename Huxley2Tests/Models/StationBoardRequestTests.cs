// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Huxley2.Models;
using OpenLDBWS;
using Xunit;

namespace Huxley2Tests.Models
{
    public class StationBoardRequestTests
    {
        [Fact]
        public void StationBoardRequestNumRowsIsDefault10()
        {
            var request = new StationBoardRequest();
            Assert.Equal(10, request.NumRows);
        }

        [Fact]
        public void StationBoardRequestNumRowsReturnsSetValue()
        {
            var request = new StationBoardRequest
            {
                NumRows = 150
            };
            Assert.Equal(150, request.NumRows);
        }

        [Fact]
        public void StationBoardRequestCrsCodeIsUpperCase()
        {
            // CRS code must be upper case in SOAP request
            var request = new StationBoardRequest
            {
                Crs = "cat",
            };
            Assert.Equal("CAT", request.Crs);
        }

        [Fact]
        public void StationBoardRequestFilterCrsCodeIsDefaultNull()
        {
            var request = new StationBoardRequest();
            Assert.Null(request.FilterCrs);
        }

        [Fact]
        public void StationBoardRequestFilterCrsCodeIsUpperCase()
        {
            // CRS code must be upper case in SOAP request
            var request = new StationBoardRequest
            {
                FilterCrs = "cat",
            };
            Assert.Equal("CAT", request.FilterCrs);
        }

        [Fact]
        public void StationBoardRequestFilterCrsCodeIsFirstInFilterList()
        {

            var request = new StationBoardRequest
            {
                FilterCrs = "cat,dog",
            };
            Assert.Equal("CAT", request.FilterCrs);
        }

        [Fact]
        public void StationBoardRequestFilterListIsDefaultEmpty()
        {
            var request = new StationBoardRequest();
            Assert.Empty(request.FilterList);
        }

        [Fact]
        public void StationBoardRequestFilterListIsUpperCase()
        {
            var request = new StationBoardRequest
            {
                FilterCrs = "cat,dog",
            };
            Assert.Equal("CAT", request.FilterList[0]);
        }

        [Fact]
        public void StationBoardRequestFilterListIsSplitOnComma()
        {
            var request = new StationBoardRequest
            {
                FilterCrs = "cat,dog",
            };
            Assert.Equal(2, request.FilterList.Count);
            Assert.Equal("CAT", request.FilterList[0]);
            Assert.Equal("DOG", request.FilterList[1]);
        }

        [Fact]
        public void StationBoardRequestFilterListIgnoresEmptyEntries()
        {
            var request = new StationBoardRequest
            {
                FilterCrs = ",cat,dog,,",
            };
            Assert.Equal(2, request.FilterList.Count);
            Assert.Equal("CAT", request.FilterList[0]);
            Assert.Equal("DOG", request.FilterList[1]);
        }

        [Fact]
        public void StationBoardRequestFilterListIgnoresWhiteSpaceEntries()
        {
            var request = new StationBoardRequest
            {
                FilterCrs = " ,cat,dog,  ,      ",
            };
            Assert.Equal(2, request.FilterList.Count);
            Assert.Equal("CAT", request.FilterList[0]);
            Assert.Equal("DOG", request.FilterList[1]);
        }

        [Fact]
        public void StationBoardRequestFilterListTrimsWhiteSpace()
        {
            var request = new StationBoardRequest
            {
                FilterCrs = " cat , dog ",
            };
            Assert.Equal(2, request.FilterList.Count);
            Assert.Equal("CAT", request.FilterList[0]);
            Assert.Equal("DOG", request.FilterList[1]);
        }

        [Fact]
        public void StationBoardRequestFilterListIgnoresDuplicates()
        {
            var request = new StationBoardRequest
            {
                FilterCrs = "cat,dog,cat",
            };
            Assert.Equal(2, request.FilterList.Count);
            Assert.Equal("CAT", request.FilterList[0]);
            Assert.Equal("DOG", request.FilterList[1]);
        }

        [Fact]
        public void StationBoardRequestFilterTypeIsDefaultTo()
        {
            var request = new StationBoardRequest();
            Assert.Equal(FilterType.to, request.FilterType);
        }

        [Fact]
        public void StationBoardRequestFilterTypeReturnsSetValue()
        {
            var request = new StationBoardRequest
            {
                FilterType = FilterType.from,
            };
            Assert.Equal(FilterType.from, request.FilterType);
        }

        [Fact]
        public void StationBoardRequestTimeOffsetIsDefault0()
        {
            var request = new StationBoardRequest();
            Assert.Equal(0, request.TimeOffset);
        }

        [Fact]
        public void StationBoardRequestTimeOffsetReturnsSetValue()
        {
            var request = new StationBoardRequest
            {
                TimeOffset = -120,
            };
            Assert.Equal(-120, request.TimeOffset);
        }

        [Fact]
        public void StationBoardRequestTimeWindowIsDefault120()
        {
            var request = new StationBoardRequest();
            Assert.Equal(120, request.TimeWindow);
        }

        [Fact]
        public void StationBoardRequestTimeWindowReturnsSetValue()
        {
            var request = new StationBoardRequest
            {
                TimeWindow = 60,
            };
            Assert.Equal(60, request.TimeWindow);
        }

        [Fact]
        public void StationBoardRequestExpandIsDefaultFalse()
        {
            var request = new StationBoardRequest();
            Assert.False(request.Expand);
        }

        [Fact]
        public void StationBoardRequestExpandReturnsSetValue()
        {
            var request = new StationBoardRequest
            {
                Expand = true,
            };
            Assert.True(request.Expand);
        }

        [Fact]
        public void StationBoardRequestStdIsDefaultEmpty()
        {
            var request = new StationBoardRequest();
            Assert.Equal(string.Empty, request.Std);
        }

        [Fact]
        public void StationBoardRequestStdReturnsSetValue()
        {
            var request = new StationBoardRequest
            {
                Std = "0729,0744,0748",
            };
            Assert.Equal("0729,0744,0748", request.Std);
        }
    }
}
