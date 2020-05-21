namespace BSOA.Demo.Model
{
    public partial class SarifLogBsoa
    {
        public override string ToString()
        {
            return $"{Result.Count:n0} {nameof(Result)}s";
        }
    }
}
