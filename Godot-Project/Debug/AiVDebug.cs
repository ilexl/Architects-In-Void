using System.Collections.Generic;
using System.Runtime.InteropServices;
using Godot;

namespace ArchitectsInVoid.Debug;

public sealed partial class AiVDebug : Node
{

    public static SceneTree SceneTree = (SceneTree)Engine.GetMainLoop();
    public static Window Root = SceneTree.GetRoot();
    
    
    AiVDebug()
    {
        Root.AddChild(this);
    }

    #region Singleton
    private static AiVDebug _instance = null;
    private static readonly object Padlock = new object();

    public static AiVDebug Instance
    {
        get
        {
            lock (Padlock)
            {
                if (_instance == null)
                {
                    _instance = new AiVDebug();
                }

                return _instance;
            }
        }
    }
    #endregion
    #region Instanced
        private Dictionary<MeshInstance3D, double> _debugObjects = new();
        private Dictionary<MeshInstance3D, double> _backList = new();
        public override void _Process(double delta)
        {
            _backList.Clear();
            
            foreach (var kvp in _debugObjects)
            {
                MeshInstance3D mesh = kvp.Key;
                bool yourTimeIsNow = kvp.Value < delta;

                if (yourTimeIsNow)
                {
                    GD.Print("Your time is now");
                    mesh.QueueFree();
                }
                else
                {
                    _backList.Add(kvp.Key, kvp.Value - delta);
                }
            }

            // Cheaper to reuse both lists than to instantiate a new one
            (_backList, _debugObjects) = (_debugObjects, _backList);
        }
    #endregion


    
    


    

    
    
    #region Mesh management
        
    private static readonly StandardMaterial3D DefaultMaterial = new()
    {
        ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
        VertexColorUseAsAlbedo = true,
        Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
        CullMode = BaseMaterial3D.CullModeEnum.Disabled,
    };
    private static void InstantiateMesh(Mesh mesh, double duration)
    {

        MeshInstance3D meshInstance = new MeshInstance3D
        {
            Mesh = mesh,
            MaterialOverride = DefaultMaterial,
        };
        Root.AddChild(meshInstance);
        Instance._debugObjects.Add(meshInstance, duration);
    }
    #endregion
    #region DebugLine
        
    
    public enum DLineType
    {
        Auto,
        Wireframe,
        Tangible
    }
    
    private const double DefaultDuration = 1.0 / 60;
    private const double DefaultThickness = 0;
    
    // Cannot make consts for structs I don't think
    
    

    private static readonly Color DefaultColor = new Color(1.0f, 1.0f, 1.0f);
    private static readonly Vector3 DefaultB = Vector3.Zero;
    public static void Line(Vector3 a, [Optional]Vector3? b, [Optional]Color? color, double duration = DefaultDuration, double thickness = DefaultThickness, DLineType type = DLineType.Auto )
    {
        Vector3 finalB = b ?? DefaultB;
        Color finalColor = color ?? DefaultColor;

        type = (type == DLineType.Auto) ? (thickness == 0 ? DLineType.Wireframe : DLineType.Tangible) : type;


        ImmediateMesh mesh = new ImmediateMesh();


        switch (type)
        {
            case DLineType.Tangible:
                // TODO: Do cross product stuff with camera
                break;
            case DLineType.Wireframe:
                mesh.SurfaceBegin(Mesh.PrimitiveType.Lines);
                mesh.SurfaceSetColor(finalColor);
                mesh.SurfaceAddVertex(a);
                mesh.SurfaceAddVertex(finalB);
                mesh.SurfaceEnd();
                break;
        }

        InstantiateMesh(mesh, duration);
        
    }

    
    #endregion
    
}