public class IsEditorReader : IReader<bool>
{
    bool IReader<bool>.Read()
    {
#if UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
