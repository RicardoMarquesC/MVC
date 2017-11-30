using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Helper
{
    public class PosicaoImagem
    {
        public int top;
        public int bottom;
        public int left;
        public int right;

        public PosicaoImagem(int t, int b, int l, int r)
        {
            this.top = t;
            this.bottom = b;
            this.left = l;
            this.right = r;
        }
    }
}