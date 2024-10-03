using Godot;

namespace ArchitectsInVoid.Debug.Meshes;

public partial class DebugMesh : MeshInstance3D
{
    public enum Type
    {
        Auto,
        Wireframe,
        Solid
    }
    #region Stuff
    private static readonly StandardMaterial3D DefaultMaterial = new()
    {
        ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
        VertexColorUseAsAlbedo = true,
        Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
        CullMode = BaseMaterial3D.CullModeEnum.Disabled,
    };
    
    protected Color Color;
    public double TimeToLive = 0;
    protected Type type;
    protected DebugDraw Instance;
    protected ImmediateMesh IM;
    #endregion
    // Prevents IDE from getting angry, should never use parameterless constructor for this and derived classes
    public DebugMesh()
    {
        
    }
    public DebugMesh(Color color, double duration, Type type, DebugDraw instance)
    {
        Color = color;
        TimeToLive = duration;
        this.type = type;
        Instance = instance;
        IM = new ImmediateMesh();
        MaterialOverride = DefaultMaterial;
        Mesh = IM;
    }
    
    /// <summary>
    /// <para>Ticks the time to live down for this object.</para>
    /// <para>Derrived objects may override this to do additional processing.</para>
    /// <para> I.E, A debug object may be infinitely thin and needs to be remade to face the camera.</para>
    /// </summary>
    /// <param name="delta">Time elapsed since last check.</param>
    /// <returns>True if it should be destroyed.</returns>
    public virtual bool Update(double delta)
    {
        TimeToLive -= delta;
        return TimeToLive <= 0;
    }
    
}