namespace AC3
{ 
    internal class Solver<T>
    {
        List<T>[] domains;
        Dictionary<int, Func<T, bool>> unaryConstraints;
        Dictionary<(int, int), Func<T, T, bool>> binaryConstraints;

        HashSet<(int, int)> workList;


        public Solver(
            List<T>[] domains,
            Dictionary<int, Func<T, bool>> unaryConstraints,
            Dictionary<(int, int), Func<T, T, bool>> binaryConstraints)
        {
            this.domains = domains;
            this.unaryConstraints = unaryConstraints;
            this.binaryConstraints = binaryConstraints;
            workList = new();
         }

        public bool Solve()
        {
            MakeInitialDomainsConsistant();
            AddArcsToWorkList();
            do
            {
                var (x, y) = workList.ElementAt(workList.RandomIndex());
                workList.Remove((x, y));

                if (ArcReduce((x, y)))
                {
                    if (domains[x].IsEmpty())
                        return false; // failed to solve

                    foreach (var arc in GetArcs(x))
                        if (arc.Item1 != y && arc.Item2 != y)
                            workList.Add(arc);
                }
            }
            while (!workList.IsEmpty());
            return true;
        }

        private bool ArcReduce((int x, int y) arc)
        {
            var changed = false;
            for(int i = domains[arc.x].Count - 1; i >= 0; i--)
            {
                var vx = domains[arc.x][i];
                bool found = false;
                for(int j = 0; j < domains[arc.y].Count; j++)
                {
                    var vy = domains[arc.y][j];
                    if (binaryConstraints[(arc.x, arc.y)](vx, vy))
                    { found = true; break; }
                }
                if (!found)
                {
                    changed = true;
                    domains[arc.x].RemoveAt(i);
                }
            }
            return changed;
        }

        private void AddArcsToWorkList()
        {
            for (int i = 0; i < domains.Length; i++)
                workList.AddAll(GetArcs(i));
        }
        private List<(int, int)> GetArcs(int i)
        {
            List<(int, int)> ret = new();
            for (int j = 0; j < domains.Length; j++)
                if (binaryConstraints.ContainsKey((i, j)) || binaryConstraints.ContainsKey((j, i)))
                    ret.AddAll([(i, j), (j, i)]);  
            return ret;
        }

        private void MakeInitialDomainsConsistant()
        {
            foreach (var c in unaryConstraints)
            {
                var variable = c.Key;
                var constraint = c.Value;
                var domain = domains[variable];
                for (int i = domain.Count - 1; i >= 0; i--)
                    if (!constraint(domain[i]))
                        domain.RemoveAt(i);
            }
        }
    }
}
