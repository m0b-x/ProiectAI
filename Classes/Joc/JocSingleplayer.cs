using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class JocSingleplayer : EngineJoc, IDisposable
    {
        private static int _intervalTimere = 50;

        private Om _jucatorServer;
        private Om _jucatorClient;

        private ParserTabla _parserTabla;
        Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaHost;
        Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaClient;
        Tuple<Pozitie, Pozitie> _penultimaMutareHost;
        Tuple<Pozitie, Pozitie> _penultimaMutareClient;

        private bool _esteRandulOmului;
        private bool _esteRandulMasinii;

        public Om JucatorOm
        {
            get { return _jucatorServer; }
        }

        public Om JucatorMasina
        {
            get { return _jucatorClient; }
        }

        public JocSingleplayer(Form parentForm, ref Tuple<Om, Om> jucatori) : base(parentForm)
        {
            AdaugaEvenimentCadrane();
            _jucatorServer = jucatori.Item1;
            _jucatorClient = jucatori.Item2;

            _esteRandulMasinii = false;
            _esteRandulOmului = false;

            _ultimaMutarePrimitaHost = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));
            _ultimaMutarePrimitaClient = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

            _penultimaMutareHost = new Tuple<Pozitie, Pozitie>(new Pozitie(2, 2), new Pozitie(2, 2));
            _penultimaMutareClient = new Tuple<Pozitie, Pozitie>(new Pozitie(2, 2), new Pozitie(2, 2));

            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala, ConstantaTabla.LungimeMesajDiferential);
        }
        public JocSingleplayer(Form parentForm, int[,] matriceTabla, ref Tuple<Om, Om> jucatori) : base(parentForm, matriceTabla)
        {
            _jucatorServer = jucatori.Item1;
            _jucatorClient = jucatori.Item2;

            _esteRandulMasinii = false;
            _esteRandulOmului = false;

            _ultimaMutarePrimitaHost = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));
            _ultimaMutarePrimitaClient = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

            _penultimaMutareHost = new Tuple<Pozitie, Pozitie>(new Pozitie(2, 2), new Pozitie(2, 2));
            _penultimaMutareClient = new Tuple<Pozitie, Pozitie>(new Pozitie(2, 2), new Pozitie(2, 2));

            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala, ConstantaTabla.LungimeMesajDiferential);
        }

        public void AdaugaEvenimentCadrane()
        {
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    ArrayCadrane[linie, coloana].Click += OnCadranClick;
                }
            }
        }

        public void EsteRandulMasinii()
        {
            _esteRandulMasinii = true;
            _esteRandulOmului = false;
        }

        public void EsteRandulOmului()
        {
            _esteRandulOmului = true;
            _esteRandulMasinii = false;
        }
        public void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitie)
        {
            if (MatriceCodPiese[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
            {
                DecoloreazaMutariPosibile(PozitiiMutariPosibile);
                ActualizeazaUltimaMutare(piesa.Pozitie, pozitie);
                MatriceCodPiese[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                SeteazaPiesaCadranului(piesa.Pozitie, ConstantaTabla.PiesaNula);

                piesa.Pozitie = pozitie;
                MatriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                SeteazaPiesaCadranului(piesa.Pozitie, piesa);

                ListaPiese.Remove(GetPiesaCuPozitia(pozitie));
                ConstantaSunet.SunetPiesaLuata.Play();
            }
            else
            {
                DecoloreazaMutariPosibile(PozitiiMutariPosibile);
                ActualizeazaUltimaMutare(piesa.Pozitie, pozitie);
                MatriceCodPiese[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                MatriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                SeteazaPiesaCadranului(piesa.Pozitie, ConstantaTabla.PiesaNula);

                piesa.Pozitie = pozitie;
                SeteazaPiesaCadranului(piesa.Pozitie, piesa);

                ConstantaSunet.SunetPiesaMutata.Play();
            }
            PiesaSelectata = ConstantaTabla.PiesaNula;
            PozitiiMutariPosibile.Clear();
        }

        ~JocSingleplayer() => Dispose();

        public void OnCadranClick(object sender, EventArgs e)
        {
            if (PiesaSelectata == ConstantaTabla.PiesaNula)
            {
                Pozitie pozitie = new Pozitie(0, 0);
                pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;


                if (ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                {
                    Piesa piesa = GetPiesaCuPozitia(pozitie);
                    piesa.ArataMutariPosibile(this);
                    if (ExistaMutariPosibile() == true)
                    {
                        ArataPiesaSelectata(piesa);
                        PiesaSelectata = piesa;
                    }
                    else
                    {
                        ArataPozitieBlocata(pozitie);
                    }
                }
            }
            else
            {
                Pozitie pozitie = new Pozitie(0, 0);
                pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;

                if (PiesaSelectata.Pozitie == pozitie)
                {
                    return;
                }
                if (EsteMutareaPosibila(pozitie))
                {
                    AscundePiesaSelectata(PiesaSelectata);
                    RealizeazaMutareaLocal(PiesaSelectata, pozitie);
                }
                else
                {
                    return;
                }
            }
        }

    }
}
