using ABCsystem.Teach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCsystem.Core
{
    public class Global : IDisposable
    {
        #region Singleton Instance
        private static readonly Lazy<Global> _instance = new Lazy<Global>(() => new Global());

        public static Global Inst
        {
            get
            {
                return _instance.Value;
            }
        }
        #endregion

        private InspStage _stage = new InspStage();

        public InspStage InspStage
        {
            get { return _stage; }
        }

        //song : 현재 선택된 ROI 기억
        public InspWindow CurTeachWindow { get; set; }


        public Global()
        {
        }

        public void Initialize()
        {
            _stage.Initialize();
        }

        public void Dispose()
        {
            _stage.Dispose();
        }
    }
}
