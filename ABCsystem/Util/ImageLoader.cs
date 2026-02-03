using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCsystem.Util
{
    public class ImageLoader : IDisposable
    {
        private List<string> _sortedImages;
        private int _grabIndex = -1;

        public int Count => (_sortedImages != null) ? _sortedImages.Count : 0;
        public int CurrentIndex => _grabIndex;

        public bool CyclicMode { get; set; } = true;

        public ImageLoader() { }

        public bool LoadImages(string imageDir)
        {
            if (!Directory.Exists(imageDir))
                return false;

            _sortedImages = ImageFileSorter.GetSortedImages(imageDir);
            if (_sortedImages.Count() <= 0)
                return false;

            _grabIndex = -1;

            return true;
        }
        public bool IsLoadedImages()
        {
            if (_sortedImages == null)
                return false;

            if (_sortedImages.Count() <= 0)
                return false;

            return true;
        }

        public bool Reset()
        {
            _grabIndex = -1;
            return true;
        }

        public string GetImagePath()
        {
            if (_sortedImages == null)
                return "";

            _grabIndex++;

            if (_grabIndex >= _sortedImages.Count)
            {
                if (CyclicMode == false)
                    return "";

                _grabIndex = 0;
            }

            return _sortedImages[_grabIndex];
        }

        public string GetNextImagePath(bool reset = false)
        {
            if (reset)
                Reset();

            return GetImagePath();
        }

        // ImageLoader.cs 내부에 추가
        public string GetCurrentImagePath()
        {
            // 리스트가 없거나 인덱스가 유효하지 않으면 빈 문자열 반환
            if (_sortedImages == null || _grabIndex < 0 || _grabIndex >= _sortedImages.Count)
                return "";

            return _sortedImages[_grabIndex];
        }

        #region Dispose

        private bool _disposed = false; // to detect redundant calls

        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;
        }
        ~ImageLoader()
        {
            Dispose(disposing: false);
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
