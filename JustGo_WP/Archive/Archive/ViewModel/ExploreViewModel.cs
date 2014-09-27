using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archive.Datas;

namespace Archive.ViewModel
{
    public class ExploreViewModel
    {
        public ObservableCollection<Goal> Explores { get; set; }
        //public Goal TopExplore { get; set; }

        public ExploreViewModel()
        {
            Explores = new ObservableCollection<Goal>();
            //TopExplore = new Goal();
        }

        public async void LoadData()
        {
            await ServerApi.GetExploreAsync(Explores);
            //TopExplore = Explores[0];
            //Explores.RemoveAt(0);
        }
    }
}
