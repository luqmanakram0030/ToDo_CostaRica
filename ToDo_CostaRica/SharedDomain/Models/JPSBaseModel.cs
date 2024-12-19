
using static ToDoCR.SharedDomain.JPSModels.JPSModels;

namespace ToDoCR.SharedDomain.Models
{
    public class JPSBaseModel
    {
        int formularioId;
        private List<LoteriaNacional> loteriaNacional;
        private List<LoteriaPopular> chances;
        private List<Lotto> lotto;
        private List<NuevosTiempos> nuevosTiempos;
        private List<TresMonazos> tresMonazos;
        private bool error;

        public List<LoteriaNacional> LoteriaNacional
        {
            get
            {
                if (loteriaNacional == null)
                    loteriaNacional = new List<LoteriaNacional>();
                return loteriaNacional;
            }
            set => loteriaNacional = value;
        }
        public List<LoteriaPopular> Chances {
            get
            {
                if (chances == null)
                    chances = new List<LoteriaPopular>();
                return chances;
            }
            set => chances = value; }
        public List<Lotto> Lotto {
            get
            {
                if (lotto == null)
                    lotto = new List<Lotto>();
                return lotto;
            }
            set => lotto = value; }
        public List<NuevosTiempos> NuevosTiempos {
            get
            {
                if (nuevosTiempos == null)
                    nuevosTiempos = new List<NuevosTiempos>();
                return nuevosTiempos;
            }
            set => nuevosTiempos = value; }
        public List<TresMonazos> TresMonazos {
            get
            {
                if (tresMonazos == null)
                    tresMonazos = new List<TresMonazos>();
                return tresMonazos;
            }
            set => tresMonazos = value; }
        public bool Error { get => error; set => error = value; }
        public int FormularioId { get => formularioId; set => formularioId = value; }

        public JPSBaseModel()
        {

        }
    }
}
