using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroGUI
{
    internal class Song
    {
        private int year;
        private string singer;
        private string title;
        private int place;
        private int score;

        public Song(int year, string singer, string title, int place, int score) {
            this.year = year;
            this.singer = singer;
            this.title = title;
            this.place = place;
            this.score = score;
        }

        public int Year { get { return year; } }
        public string Title { get { return title; } }
        public int Place { get { return place; } }
        public int Score { get { return score; } }
        public string Singer { get { return singer; } }
          

    }
}
