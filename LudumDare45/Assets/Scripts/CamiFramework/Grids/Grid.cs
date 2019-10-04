using Cami.Collections;
using CamiFramwork.ConsoleUtil;
using CamiFramwork.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamiFramwork.Grids
{
    public class Grid<T>
    {
        public void InitGrid(int cellsPerAxis, float cellSize, CreateCellDelegate cellCreateFunc)
        {
            InitGrid(cellsPerAxis, cellsPerAxis, cellsPerAxis, cellSize, cellCreateFunc);
        }

        public virtual void InitGrid(int cellsPerXAxis, int cellsPerYAxis, int cellsPerZAxis, float cellSize, CreateCellDelegate cellCreateFunc)
        {
            this.cellsPerXAxis = cellsPerXAxis;
            this.cellsPerYAxis = cellsPerYAxis;
            this.cellsPerZAxis = cellsPerZAxis;
            this.cellSize = cellSize;
            this.cellCreateFunc = cellCreateFunc;
            
            cells = new T[cellsPerXAxis, cellsPerYAxis, cellsPerZAxis];

            for(int y = 0; y < cellsPerYAxis; ++y)
            {
                for (int z = 0; z < cellsPerZAxis; ++z)
                {
                    for(int x = 0; x < cellsPerXAxis; ++x)
                    {
                        T cell = cellCreateFunc(this, x, y, z);
                        cells[x, y, z] = cell;
                    }
                }
            }

            bounds = BoundsExt.New(Vector3.zero, new Vector3(
                cellSize * (float)cellsPerXAxis,
                cellSize * (float)cellsPerYAxis,
                cellSize * (float)cellsPerZAxis));
        }
        
        #region Cells
        public void GetCellIndices(Vector3 position, out int x, out int y, out int z)
        {
            position -= this.Position;
            x = (int)(position.x / cellSize);
            y = (int)(position.y / cellSize);
            z = (int)(position.z / cellSize);
            
            x = position.x < 0 ? x - 1 : x;
            y = position.y < 0 ? y - 1 : y;
            z = position.z < 0 ? z - 1 : z;
        }
        
        public bool IsValidCell(Vector3 position)
        {
            int x, y, z;
            GetCellIndices(position, out x, out y, out z);

            return IsValidCell(x, y, z);
        }

        public bool IsValidCell(int x, int y, int z)
        {
            if (x < 0 || x >= cellsPerXAxis)
                return false;
            if (z < 0 || z >= cellsPerZAxis)
                return false;
            if (y < 0 || y >= cellsPerYAxis)
                return false;

            return true;
        }
        
        public T GetCell(Vector3 position)
        {
            int x, y, z;
            GetCellIndices(position, out x, out y, out z);

            return GetCell(x, y, z);
        }

        public T GetCell(int x, int y, int z)
        {
            if (!IsValidCell(x, y, z))
                return default(T);

            return cells[x, y, z];
        }

        public T GetCellNoCheck(int x, int y, int z)
        {
            return cells[x, y, z];
        }

        public void SetCell(Vector3 position, T value)
        {
            int x, y, z;
            GetCellIndices(position, out x, out y, out z);

            SetCell(x, y, z, value);
        }

        public void SetCell(int x, int y, int z, T value)
        {
            if (!IsValidCell(x, y, z))
                return;

            cells[x, y, z] = value;

            if(CellChanged != null)
                CellChanged(x, y, z, value);
        }

        public void SetCellNoCheck(int x, int y, int z, T value)
        {
            cells[x, y, z] = value;
            
            if(CellChanged != null)
                CellChanged(x, y, z, value);
        }

        public Bounds GetCellBounds(int x, int y, int z)
        {
            var min = new Vector3(x * CellSize, y * CellSize, z * CellSize) + Position;
            var max = min + Vector3Ext.New(CellSize);
            return BoundsExt.New(min, max);
        }

        public Vector3 GetCellFaceNormal(int x, int y, int z, Vector3 point)
        {
            Bounds cellBounds = GetCellBounds(x, y, z);
            return cellBounds.GetClosestFaceNormal(point);
        }
        
        public int CastCells(Ray ray, float maxDistance, ArrayList<CellCastInfo> results,
            System.Func<CellCastInfo, bool> castCondition)
        {
            return CastCells(ray.origin, ray.direction, maxDistance, results, castCondition);
        }

        public int CastCells(Vector3 origin, Vector3 dir, float maxDistance, ArrayList<CellCastInfo> results, 
            System.Func<CellCastInfo, bool> castCondition)
        {
            int hitCount = 0;
            foreach(CellCastInfo castInfo in CastCells(origin, dir, maxDistance))
            {
                if (castCondition != null && castCondition(castInfo))
                    return hitCount;

                results.Add(castInfo);
                ++hitCount;
            }

            return hitCount;
        }
        
        public CellCastInfo GetCastInfo(int x, int y, int z)
        {
            return new CellCastInfo(x, y, z, cells[x, y, z], Vector3.zero, Vector3.zero);
        }

        public IEnumerable<CellCastInfo> CastCells(Ray ray, float maxDistance)
        {
            return CastCells(ray.origin, ray.direction, maxDistance);
        }

        public IEnumerable<CellCastInfo> CastCells(Vector3 origin, Vector3 dir, float maxDistance)
        {
            int currentX, currentY, currentZ;
            Vector3 intersectPoint, faceNormal;
            T startCell, endCell, currentCell;

            var ray = new Ray(origin, dir);

            GetCellIndices(origin, out currentX, out currentY, out currentZ);

            intersectPoint = Vector3.zero;

            float distance = 0f;
            Vector3 normal;
            if(IsValidCell(currentX, currentY, currentZ)) // Started inside a cell
            {
                intersectPoint = origin;
                startCell = cells[currentX, currentY, currentZ];
            }
            else
            {
                if(bounds.Intersects(ray, maxDistance, out normal, out distance) == false)
                {
                    //Console.Log("Does not intersect grid {0}", this);
                    yield break;
                }

                distance = Mathf.Abs(distance);

                intersectPoint = origin + (dir * distance);
                origin = intersectPoint + (dir * 0.001f);
                GetCellIndices(origin, out currentX, out currentY, out currentZ);

                if (!IsValidCell(currentX, currentY, currentZ))
                    yield break;

                startCell = cells[currentX, currentY, currentZ];
            }

            var reverseRay = new Ray(ray.origin + (dir * maxDistance), -dir);
        
            if(bounds.Intersects(ray, maxDistance, out normal, out distance) == false)
            {
                //Console.Log("Does not intersect grid reverse {0}", this);
                yield break;
            }

            //Console.Log("R: {0}, d: {1}", ray, distance);    
            distance = Mathf.Abs(distance);

            Vector3 endPoint = reverseRay.origin + (reverseRay.direction * (distance + 0.001f));
            endCell = GetCell(endPoint);
            
            faceNormal = GetCellFaceNormal(currentX, currentY, currentZ, intersectPoint);
            var castInfo = new CellCastInfo(currentX, currentY, currentZ, startCell, intersectPoint, faceNormal);
            yield return castInfo;

            Vector3 current = (origin - Position) / CellSize;

            currentX = (int)current.x;
            currentY = (int)current.y;
            currentZ = (int)current.z;
            GetCellIndices(origin, out currentX, out currentY, out currentZ);

            int stepX, stepY, stepZ;
            stepX = dir.x > 0 ? 1 : (dir.x < 0 ? -1 : 0);
            stepY = dir.y > 0 ? 1 : (dir.y < 0 ? -1 : 0);
            stepZ = dir.z > 0 ? 1 : (dir.z < 0 ? -1 : 0);

            Vector3 rayDelta = dir * (maxDistance);

            Vector3 tDelta = Vector3.zero; // size of voxel in terms of t            
            if(stepX != 0) { tDelta.x = CellSize / rayDelta.x; }
            else { tDelta.x = 10000000.0f; }

            if(stepY != 0) { tDelta.y = CellSize / rayDelta.y; }
            else { tDelta.y = 10000000.0f; }

            if(stepZ != 0) { tDelta.z = CellSize / rayDelta.z; }
            else { tDelta.z = 10000000.0f; }

            Vector3 tMax = Vector3.zero; // distance to next boundary (in terms of t)
            if (stepX > 0) { tMax.x = tDelta.x * MathsHelper.FracPos(current.x); }
            else { tMax.x = tDelta.x * MathsHelper.FracNeg(current.x); }
            
            if (stepY > 0) { tMax.y = tDelta.y * MathsHelper.FracPos(current.y); }
            else { tMax.y = tDelta.y * MathsHelper.FracNeg(current.y); }
            
            if (stepZ > 0) { tMax.z = tDelta.z * MathsHelper.FracPos(current.z); }
            else { tMax.z = tDelta.z * MathsHelper.FracNeg(current.z); }
            
            currentCell = startCell;
            int iters = 0;
            const int MaxIters = 10000;
            Directions movementDirection;
            while(IsValidCell(currentX, currentY, currentZ) && iters < MaxIters)
            {
                ++iters;
#if DEBUG
                if (iters == MaxIters)
                    Console.LogWarning("Grid cell cast hit max iters");
#endif

                if(Mathf.Abs(tMax.x) < Mathf.Abs(tMax.y))
                {
                    if(Mathf.Abs(tMax.x) < Mathf.Abs(tMax.z))
                    {
                        if(stepX > 0) { movementDirection = Directions.Left; }
                        else { movementDirection = Directions.Right; }

                        currentX += stepX;
                        tMax.x += tDelta.x;
                    }
                    else
                    {
                        if(stepZ > 0) { movementDirection = Directions.Forward; }
                        else { movementDirection = Directions.Backward; }

                        currentZ += stepZ;
                        tMax.z += tDelta.z;
                    }
                }
                else
                {
                    if(Mathf.Abs(tMax.y) < Mathf.Abs(tMax.z))
                    {
                        if(stepY > 0) { movementDirection = Directions.Down; }
                        else { movementDirection = Directions.Up; }

                        currentY += stepY;
                        tMax.y += tDelta.y;
                    }
                    else
                    {
                        if(stepZ > 0) { movementDirection = Directions.Forward; }
                        else { movementDirection = Directions.Backward; }

                        currentZ += stepZ;
                        tMax.z += tDelta.z;
                    }
                }

                if (!IsValidCell(currentX, currentY, currentZ))
                    break;

                currentCell = cells[currentX, currentY, currentZ];
                intersectPoint = tMax;
                faceNormal = GetDirectionVector(movementDirection);

                Bounds cellBounds = GetCellBounds(currentX, currentY, currentZ);
                if(cellBounds.IntersectRay(ray, out distance))
                {
                    intersectPoint = ray.origin + (ray.direction * distance);
                }
                if (distance > maxDistance)
                {
                    //Console.Log("Over max distance {0}", this);
                    yield break;
                }

                castInfo = new CellCastInfo(currentX, currentY, currentZ, currentCell, intersectPoint, faceNormal);
                
                yield return castInfo;
            }
        }

        public void ForeachCell(CellCallback action)
        {
            for(int y = 0; y < cellsPerYAxis; ++y)
            {
                for (int z = 0; z < cellsPerZAxis; ++z)
                {
                    for(int x = 0; x < cellsPerXAxis; ++x)
                    {
                        action(x, y, z, cells[x, y, z]);
                    }
                }
            }
        }
        
        public void ProcessCellFill(int x, int y, int z, CellCallback action, CellPredicate predicate)
        {
            if (!IsValidCell(x, y, z))
                return;

            bool[,,] closed = new bool[CellsPerXAxis, CellsPerYAxis, CellsPerZAxis];

            var baseCell = GetCastInfo(x, y, z);
            var open = new Queue<CellCastInfo>();
            open.Enqueue(baseCell);
            closed[x, y, z] = true;

            CellCastInfo cell = baseCell;

            if (!predicate(cell.X, cell.Y, cell.Z, cell.Cell))
                return;

            action(cell.X, cell.Y, cell.Z, cell.Cell);

            while (open.Count > 0)
            {
                CellCastInfo next = open.Dequeue();

                int west, east;
                bool match;

                // Find west
                x = next.X;
                y = next.Y;
                z = next.Z;
                match = true;

                while (match)
                {
                    ++x;
                    if (IsValidCell(x, y, z))
                    {
                        cell = GetCastInfo(x, y, z);
                        match = predicate(cell.X, cell.Y, cell.Z, cell.Cell);
                    }
                    else
                    {
                        match = false;
                    }
                }
                west = x;
                //

                // Find east
                x = next.X;
                y = next.Y;
                z = next.Z;
                match = true;

                while (match)
                {
                    --x;
                    if (IsValidCell(x, y, z))
                    {
                        cell = GetCastInfo(x, y, z);
                        match = predicate(cell.X, cell.Y, cell.Z, cell.Cell);
                    }
                    else
                    {
                        match = false;
                    }
                }
                east = x;
                //

                for (x = east + 1; x < west; ++x)
                {
                    if (closed[x, y, z])
                        continue;
                
                    cell = GetCastInfo(x, y, z);
                    match = predicate(cell.X, cell.Y, cell.Z, cell.Cell);

                    closed[x, y, z] = true;

                    if (IsValidCell(x, y - 1, z))
                    {
                        cell = GetCastInfo(x, y - 1, z);
                        if (predicate(cell.X, cell.Y, cell.Z, cell.Cell))
                            open.Enqueue(cell);
                    }
                    if (IsValidCell(x, y + 1, z))
                    {
                        cell = GetCastInfo(x, y + 1, z);
                        if (predicate(cell.X, cell.Y, cell.Z, cell.Cell))
                            open.Enqueue(cell);
                    }
                }
            }
        }
        
        public IEnumerable<CellCastInfo> ForCellLine(
            int startX, int startY, int startZ,
            int endX, int endY, int endZ,
            bool connectDiagonal)
        {
            int dx, dy, dz;
            int sx, sy, sz;
            int accum, accum2;//accumilator

            dx = endX - startX;//Start X subtracted from End X
            dy = endY - startY;
            dz = endZ - startZ;

            sx = ((dx) < 0 ? -1 : ((dx) > 0 ? 1 : 0));//if dx is less than 0, sx = -1; otherwise if dx is greater than 0, sx = 1; otherwise sx = 0
            sy = ((dy) < 0 ? -1 : ((dy) > 0 ? 1 : 0));
            sz = ((dz) < 0 ? -1 : ((dz) > 0 ? 1 : 0));

            //dx = (dx < 0 ? -dx : dx);//if dx is less than 0, dx = -dx (becomes positive), otherwise nothing changes
            dx = Mathf.Abs(dx);//Absolute value
            //dy = (dy < 0 ? -dy : dy);
            dy = Mathf.Abs(dy);

            dz = Mathf.Abs(dz);

            endX += sx;//Add sx to End X
            endY += sy;
            endZ += sz;

            if (dx > dy)//if dx is greater than dy
            {
                if (dx > dz)
                {
                    accum = dx >> 1;
                    accum2 = accum;
                    do
                    {
                        yield return GetCastInfo(startX, startY, startZ);

                        accum -= dy;
                        accum2 -= dz;
                        if (accum < 0)
                        {
                            accum += dx;
                            startY += sy;
                        }
                        if (accum2 < 0)
                        {
                            accum2 += dx;
                            startZ += sz;
                        }
                        startX += sx;
                    }
                    while (startX != endX);
                }
                else
                {
                    accum = dz >> 1;
                    accum2 = accum;
                    do
                    {
                        yield return GetCastInfo(startX, startY, startZ);

                        accum -= dy;
                        accum2 -= dx;
                        if (accum < 0)
                        {
                            accum += dz;
                            startY += sy;
                        }
                        if (accum2 < 0)
                        {
                            accum2 += dz;
                            startX += sx;
                        }
                        startZ += sz;
                    }
                    while (startZ != endZ);
                }
            }
            else
            {
                if (dy > dz)
                {
                    accum = dy >> 1;
                    accum2 = accum;
                    do
                    {
                        yield return GetCastInfo(startX, startY, startZ);

                        accum -= dx;
                        accum2 -= dz;
                        if (accum < 0)
                        {
                            accum += dx;
                            startX += sx;
                        }
                        if (accum2 < 0)
                        {
                            accum2 += dx;
                            startZ += sz;
                        }
                        startY += sy;
                    }
                    while (startY != endY);
                }
                else
                {
                    accum = dz >> 1;
                    accum2 = accum;
                    do
                    {
                        yield return GetCastInfo(startX, startY, startZ);

                        accum -= dx;
                        accum2 -= dy;
                        if (accum < 0)
                        {
                            accum += dx;
                            startX += sx;
                        }
                        if (accum2 < 0)
                        {
                            accum2 += dx;
                            startY += sy;
                        }
                        startZ += sz;
                    }
                    while (startZ != endZ);
                }
            }
        }
        #endregion Cells

        #region Directions
        public Vector3 GetDirectionVector(Directions direction)
        {
            switch (direction)
            {
                case Directions.Left:
                    return Vector3.left;
                case Directions.Right:
                    return Vector3.right;
                case Directions.Up:
                    return Vector3.up;
                case Directions.Down:
                    return Vector3.down;
                case Directions.Forward:
                    return Vector3.back;
                case Directions.Backward:
                    return Vector3.forward;
            }
            return Vector3.zero;
        }
        #endregion Directions

        #region Properties
        public Vector3 Position
        {
            get { return bounds.min; }
            set
            {
                bounds = new Bounds(value + bounds.extents, bounds.size);
            }
        }
        public Bounds Bounds { get { return bounds; } }
        public int CellsPerXAxis { get { return cellsPerXAxis; } }
        public int CellsPerYAxis { get { return cellsPerYAxis; } }
        public int CellsPerZAxis { get { return cellsPerZAxis; } }
        public float CellSize { get {  return cellSize; } }

        public T this[int x, int y, int z]
        {
            get { return GetCellNoCheck(x, y, z); }
            set { SetCellNoCheck(x, y, z, value); }
        }

        public IEnumerable<T> Cells
        {
            get
            {
                for(int y = 0; y < cellsPerYAxis; ++y)
                {
                    for (int z = 0; z < cellsPerZAxis; ++z)
                    {
                        for (int x = 0; x < cellsPerXAxis; ++x)
                        {
                            yield return cells[x, y, z];
                        }
                    }
                }
            }
        }
        #endregion Properties

        #region Fields
        private T[,,] cells;
        private int cellsPerXAxis = 16;
        private int cellsPerYAxis = 16;
        private int cellsPerZAxis = 16;
        private float cellSize = 1f;
        private Bounds bounds;
       // private Vector3 position;
        
        public delegate T CreateCellDelegate(Grid<T> grid, int x, int y, int z);
        private CreateCellDelegate cellCreateFunc;
        #endregion Fields

        #region Events 
        public delegate void CellCallback(int x, int y, int z, T cell);
        public delegate bool CellPredicate(int x, int y, int z, T cell);
        public event CellCallback CellChanged;
        #endregion Events

        #region Enums
        #endregion Enums

        public struct CellCastInfo
        {
            public CellCastInfo(int x, int y, int z, T cell, Vector3 intersectPoint, Vector3 faceNormal)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
                this.Cell = cell;
                this.IntersectPoint = intersectPoint;
                this.FaceNormal = faceNormal;
            }

            public int X;
            public int Y;
            public int Z;

            public Vector3 IntersectPoint;
            public Vector3 FaceNormal;

            public T Cell;
        }
    }

    public enum Directions
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Backward
    }
}