using System.Collections.Generic;
using System.Runtime.InteropServices;
using Godot;

namespace ArchitectsInVoid.Debug;

public sealed partial class DebugDraw : Node
{

    public static SceneTree SceneTree = (SceneTree)Engine.GetMainLoop();
    public static Window Root = SceneTree.GetRoot();
    
    
    DebugDraw()
    {
        Root.AddChild(this);
    }

    #region Singleton
    private static DebugDraw _instance = null;
    private static readonly object Padlock = new object();

    public static DebugDraw Instance
    {
        get
        {
            lock (Padlock)
            {
                if (_instance == null)
                {
                    _instance = new DebugDraw();
                }

                return _instance;
            }
        }
    }
    #endregion
    #region Instanced

    
    private bool _cameraNormalThisFrame = false;
    private Vector3 _cameraNormal;
    private Vector3 CameraNormal
    {
        get
        {
            if (!_cameraNormalThisFrame)
            {
                _cameraNormal = Root.GetCamera3D().GlobalTransform.Basis.Z;
                _cameraNormalThisFrame = true;
            }
            return _cameraNormal;
        }
    }

    private Dictionary<MeshInstance3D, double> _debugObjects = new();
        private Dictionary<MeshInstance3D, double> _backList = new();
        public override void _Process(double delta)
        {
            _cameraNormalThisFrame = false;
            
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

    private const double DefaultDuration = 1.0 / 60; // TODO: Replace this with update rate from project settings
    private const double DefaultThickness = 0;
    
    // Cannot make consts for structs I don't think
    
    

    private static readonly Color DefaultColor = new Color(1.0f, 1.0f, 1.0f);
    private static readonly Vector3 DefaultPos = Vector3.Zero;
    /// <summary>
    /// Draws a line between two points with optional parameters.
    /// </summary>
    /// <param name="start">Start position of line. Defaults to Vector3.Zero.</param>
    /// <param name="end">End position of line. Defaults to Vector3.Zero.</param>
    /// <param name="color">Color of line. Defaults to White.</param>
    /// <param name="duration">Time until the line gets destroyed. Defaults to 1 frame.</param>
    /// <param name="thickness">Apparent "radius" of the line. Defaults to 0.</param>
    /// <param name="type">Auto, Tangible, or Wireframe. Tangible produces a line with a radius, wireframe produces an infinitely small yet visible line. Auto is Wireframe if 0, otherwise tangible. Defaults to Auto.</param>
    public static void Line([Optional]Vector3? start, [Optional]Vector3? end, [Optional]Color? color, double duration = DefaultDuration, double thickness = DefaultThickness, DLineType type = DLineType.Auto )
    {
        Vector3 a = start ?? DefaultPos;
        Vector3 b = end ?? DefaultPos;
        Color finalColor = color ?? DefaultColor;

        type = (type == DLineType.Auto) ? (thickness == 0 ? DLineType.Wireframe : DLineType.Tangible) : type;


        ImmediateMesh mesh = new ImmediateMesh();


        switch (type)
        {
            case DLineType.Tangible:
                UpdateLineWidth(mesh, a, b, thickness, finalColor, Instance.CameraNormal);
                break;
            case DLineType.Wireframe:
                mesh.SurfaceBegin(Mesh.PrimitiveType.Lines);
                mesh.SurfaceSetColor(finalColor);
                mesh.SurfaceAddVertex(a);
                mesh.SurfaceAddVertex(b);
                mesh.SurfaceEnd();
                break;
        }

        InstantiateMesh(mesh, duration);
        
    }

    
    
    #endregion
    
}