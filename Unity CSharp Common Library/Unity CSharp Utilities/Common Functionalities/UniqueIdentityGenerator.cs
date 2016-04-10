public static class UniqueIdentityGenerator
{
    private static int nextIdentity = 0;

    public static void Reset()
    {
        nextIdentity = 0;
    }

    public static int Next()
    {
        return nextIdentity++; //takes value first, then increments.
    }
}