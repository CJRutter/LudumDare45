
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CamiFramwork.Grids
{
    public class GridCell<T>
    {
        public GridCell()
        {
        }

        public virtual void Init(int x, int y, int z)
        {
            this.CellX = x;
            this.CellY = y;
            this.CellZ = z;
        }

        #region Properties
        public int CellX { get; private set; }
        public int CellY { get; private set; }
        public int CellZ { get; private set; }
        public Vector3 Position { get { return bounds.min; } }
        public Bounds Bounds { get { return bounds; } }
        public T Value
        {
            get { return value; }
            set
            {
                this.value = value;

                if(OnValueChanged != null)
                    OnValueChanged(this);
            }
        }
        #endregion Properties

        #region Fields
        private Bounds bounds;
        private T value;
        #endregion Fields

        #region Events
        public event Action<GridCell<T>> OnValueChanged;
        #endregion Events
    }
}
