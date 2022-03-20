using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{

    public class JocDeSah
    {
        private Form _parentForm;
        private Tabla _tabla;

        private int[,] _matriceCodPiese;

        private List<Piesa> _listaPiese;
        private List<Pozitie> _pozitiiMutariPosibile;
     
        private Piesa _piesaSelectata;

        private Pozitie[] _ultimaMutare;

        public List<Piesa> ListaPiese
        {
            get { return _listaPiese; }
            set { _listaPiese = value; }
        }

        public Form ParentForm
        {
            get { return _parentForm; }
            set { _parentForm = value; }
        }

        public Tabla TablaJoc
        {
            get { return _tabla; }
        }

        public Cadran[,] ArrayCadrane
        {
            get { return _tabla.ArrayCadrane; }
            set { _tabla.ArrayCadrane = value; }
        }

        public int[,] MatriceCodPiese
        {
            get { return _matriceCodPiese; }
            set { _matriceCodPiese = value; }
        }

        public int MarimeVerticala
        {
            get { return _tabla.MarimeVerticala; }
        }
        public int MarimeOrizontala
        {
            get { return _tabla.MarimeOrizontala; }
        }
        public int PragRau
        {
            get { return _tabla.PragRau; }
        }
        public int MarimeRau
        {
            get { return _tabla.MarimeRau; }
        }
        public Color CuloareCadranPar
        {
            get { return _tabla.CuloareCadranPar; }
        }

        public Color CuloareCadranImpar
        {
            get { return _tabla.CuloareCadranImpar; }
        }

        public Color CuloareCadranSelectat
        {
            get { return _tabla.CuloareCadranSelectat; }
            set { _tabla.CuloareCadranSelectat = value; }
        }

        public Color CuloareCadranMutari
        {
            get { return _tabla.CuloareCadranMutari; }
            set { _tabla.CuloareCadranMutari = value; }
        }
        public Piesa PiesaSelectata
        {
            get { return _piesaSelectata; }
            set { _piesaSelectata = value; }
        }

        public Pozitie[] UltimaMutare
        {
            get { return _ultimaMutare; }
        }

        public JocDeSah(Form parentForm)
        {
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>();
            _pozitiiMutariPosibile = new List<Pozitie>();

            _tabla = new Tabla();

            _matriceCodPiese = new int[ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala];

            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    ArrayCadrane[linie, coloana] = new Cadran(this, new Pozitie(linie, coloana), _tabla.DecideCuloareaCadranului(linie, coloana));
                    ArrayCadrane[linie, coloana].AddClickEventHandler(OnCadranClick);
                }
            }
        }
        public JocDeSah(Form parentForm,int[,] matriceTabla)
        {
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>();
            _pozitiiMutariPosibile = new List<Pozitie>();


            _tabla = new Tabla();

            _matriceCodPiese = new int[ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala];

            ArrayCadrane = new Cadran[ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala];

            _ultimaMutare = new Pozitie[2];

            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    ArrayCadrane[linie, coloana] = new Cadran( this, new Pozitie(linie, coloana), _tabla.DecideCuloareaCadranului(linie,coloana));
                    ArrayCadrane[linie, coloana].AddClickEventHandler(OnCadranClick);
                    if(matriceTabla[linie, coloana] != (int)CodPiesa.Gol)
                    {
                        Piesa piesa = ConvertesteCodPiesaInObiect((CodPiesa)Enum.ToObject(typeof(CodPiesa), matriceTabla[linie, coloana]));
                        AdaugaPiesa(ref piesa, new Pozitie(linie, coloana));
                    }
                }
            }
        }

        private Piesa ConvertesteCodPiesaInObiect(CodPiesa codPiesa)
        {
            switch (codPiesa)
            {
                case CodPiesa.PionAlb: return new Pion(Culoare.Alb);
                case CodPiesa.PionAlbastru: return new Pion(Culoare.Albastru);
                case CodPiesa.TuraAlba: return new Tura(Culoare.Alb);
                case CodPiesa.TuraAlbastra: return new Tura(Culoare.Albastru);
                case CodPiesa.TunAlb: return new Tun(Culoare.Alb);
                case CodPiesa.TunAlbastru: return new Tun(Culoare.Albastru);
                case CodPiesa.GardianAlb: return new Gardian(Culoare.Alb);
                case CodPiesa.GardianAlbastru: return new Gardian(Culoare.Albastru);
                case CodPiesa.ElefantAlb: return new Elefant(Culoare.Alb);
                case CodPiesa.ElefantAlbastru: return new Elefant(Culoare.Albastru); ;
                case CodPiesa.CalAlb: return new Cal(Culoare.Alb);
                case CodPiesa.CalAbastru: return new Cal(Culoare.Albastru);
                case CodPiesa.RegeAlb: return new Rege(Culoare.Alb);
                case CodPiesa.RegeAlbastru: return new Rege(Culoare.Albastru);
                default: return null;
            }

        }

        public void AdaugaPiesa(ref Piesa piesa, Pozitie pozitie)
        {
            if (pozitie.Linie > MarimeVerticala || pozitie.Coloana > MarimeOrizontala || pozitie.Linie < 0 || pozitie.Coloana < 0)
            {
                Debug.WriteLine("Linie sau coloana invalida! Linie:" + pozitie.Linie + ", Coloana:" + pozitie.Coloana);
            }
            else if(piesa == ConstantaTabla.PiesaNula)
            {
                Debug.WriteLine("Piesa invalida! Linie:" + pozitie.Linie + ", Coloana:" + pozitie.Coloana);
            }
            else
            {
                if (piesa.PusaPeTabla == true)
                {
                    Debug.WriteLine("Eroare:Piesa selectata este deja pusa pe tabla!");
                }
                else
                {
                    if (_matriceCodPiese[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
                    {
                        Debug.WriteLine("Eroare:adugare piesa peste alta! Linie:{0},Coloana:{1},PiesaAdaugata:{2}",pozitie.Linie,pozitie.Coloana,piesa.Cod);
                    }
                    else
                    {
                        piesa.Pozitie = pozitie;

                        piesa.PusaPeTabla = true;

                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);
                        this.MatriceCodPiese[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)piesa.Cod;
                        this.ListaPiese.Add(piesa);
                    }
                }
            }
        }

        public void ArataPiesaSelectata(Piesa piesa)
        {
            ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].BackColor = CuloareCadranSelectat;
        }
        public void ArataPiesaBlocata(Pozitie pozitie)
        {
            ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = ConstantaTabla.CuloarePiesaBlocata;
            System.Timers.Timer timerPiesaBlocata = new System.Timers.Timer();
            timerPiesaBlocata.Elapsed += new ElapsedEventHandler((sender, e) => DecoloreazaPiesaBlocata(sender, e, pozitie, timerPiesaBlocata));
            timerPiesaBlocata.Interval = 400;
            timerPiesaBlocata.Enabled = true;
            ConstantaSunet.SunetMutareGresita.Play();
        }
        private void DecoloreazaPiesaBlocata(object source, ElapsedEventArgs e,Pozitie pozitie, System.Timers.Timer timer)
        {
            ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = _tabla.DecideCuloareaCadranului(pozitie.Linie, pozitie.Coloana);
            timer.Stop();
        }

        public void AscundePiesaSelectata(Piesa piesa)
        {
            ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].BackColor = _tabla.DecideCuloareaCadranului(piesa.Pozitie.Linie,piesa.Pozitie.Coloana);
        }


        public void DecoloreazaMutariPosibile(List<Pozitie> pozitii)
        {
            if (_pozitiiMutariPosibile != null)
            {
                foreach (Pozitie pozitie in _pozitiiMutariPosibile)
                {
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = _tabla.DecideCuloareaCadranului(pozitie.Linie, pozitie.Coloana);
                }
            }
        }

        public bool ExistaMutariPosibile()
        {
            if (_pozitiiMutariPosibile == null || _pozitiiMutariPosibile.Count == 0)
                return false;
            return true;
        }
        public void ColoreazaMutariPosibile(List<Pozitie> pozitii)
        {
            if (pozitii != null)
            {
                foreach (Pozitie pozitie in pozitii)
                {
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = CuloareCadranMutari;
                    _pozitiiMutariPosibile.Add(new Pozitie(pozitie.Linie, pozitie.Coloana));
                }
            }
        }
        public bool EsteMutareaPosibila(Pozitie pozitie)
        {
            if (ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor == CuloareCadranMutari)
                return true;
            else
                return false;
        }

        public void ActualizeazaUltimaMutare(Pozitie pozitieInitiala,Pozitie pozitiFinala)
        {
            _ultimaMutare[0] = pozitieInitiala;
            _ultimaMutare[1] = pozitiFinala;
        }

        public void MutaPiesa(Piesa piesa, Pozitie pozitie)
        {
            if (pozitie.Linie > MarimeVerticala || pozitie.Coloana > MarimeOrizontala || pozitie.Linie < 0 || pozitie.Coloana < 0)
            {
                Debug.WriteLine("Linie sau coloana invalida! Linie:" + pozitie.Linie + ", Coloana:" + pozitie.Coloana);
            }
            else
            {
                if (piesa.PusaPeTabla == false)
                {
                    Debug.WriteLine("Eroare:Piesa nu este pusa pe tabla!");
                }
                else
                {
                    if (_matriceCodPiese[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
                    {
                        _listaPiese.Remove(GetPiesaCuPozitia(pozitie));
                        _matriceCodPiese[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(ConstantaTabla.PiesaNula);

                        ActualizeazaUltimaMutare(new Pozitie(piesa.Pozitie.Linie, piesa.Pozitie.Coloana), new Pozitie(pozitie.Linie, pozitie.Coloana));

                        piesa.Pozitie = pozitie;
                        _matriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);

                        ConstantaSunet.SunetPiesaLuata.Play();
                    }
                    else
                    {
                        _matriceCodPiese[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                        _matriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(ConstantaTabla.PiesaNula);

                        piesa.Pozitie = pozitie;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);

                        ConstantaSunet.SunetPiesaMutata.Play();
                    }
                    DecoloreazaMutariPosibile(_pozitiiMutariPosibile);
                    _piesaSelectata = ConstantaTabla.PiesaNula;
                    _pozitiiMutariPosibile.Clear();
                }
            }
        }

        public Piesa GetPiesaCuPozitia(Pozitie pozitie)
        {
            if (_listaPiese != null)
            {
                foreach (Piesa piesa in _listaPiese)
                {
                    if (piesa.Pozitie == pozitie)
                        return piesa;
                }
            }
            return null;
        }

        public void CurataTabla()
        {
            for (int linie = 0; linie < _tabla.MarimeOrizontala; linie++)
            {
                for (int coloana = 0; coloana < _tabla.MarimeVerticala; coloana++)
                {
                    ArrayCadrane[linie, coloana].setPiesa(ConstantaTabla.PiesaNula);
                }
            }
        }
        public void OnCadranClick(object sender, EventArgs e)
        {
            if (_piesaSelectata == ConstantaTabla.PiesaNula)
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
                        Debug.WriteLine("Click Piesa:" + piesa.Cod + "->[linie:" + piesa.Pozitie.Linie + ",coloana:" + piesa.Pozitie.Coloana + "]");
                        _piesaSelectata = piesa;
                    }
                    else 
                    {
                        ArataPiesaBlocata(pozitie);
                        return;
                    }
                }
                else
                {
                    Debug.WriteLine("Click Piesa:Gol->[linie:" + pozitie.Linie + ",coloana:" + pozitie.Coloana + "]");
                }
            }
            else
            {
                Pozitie pozitie = new Pozitie(0, 0);
                pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;

                if (_piesaSelectata.Pozitie == pozitie)
                {
                    return;
                }
                if (EsteMutareaPosibila(pozitie))
                {
                    AscundePiesaSelectata(_piesaSelectata);
                    MutaPiesa(_piesaSelectata, pozitie);
                }
                else
                {
                    return;
                }
            }
        }
    }
}
