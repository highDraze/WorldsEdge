using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LowPolyWater : MonoBehaviour {

    public Material material;
    public int sizeX = 30;
    public int sizeZ = 30;

    bool generate;

    const int maxVerts = ushort.MaxValue;
    const float sin60 = 0.86602540378f;
    const float inv_tan60 = 0.57735026919f;
    

    void Start() {
        if (material == null || !material.HasProperty("_ShoreBlend")) return;
        if (material.GetFloat("_ShoreBlend") > 0.1f) {
            Camera.main.depthTextureMode |= DepthTextureMode.Depth;
        }
        OnValidate();
    }
    // the code below only compiles inside the editor!

    void OnValidate() {
        generate = true;
        sizeX = Mathf.Clamp(sizeX, 1, 256);
        sizeZ = Mathf.Clamp(sizeZ, 1, 256);
        var mfs = GetComponentsInChildren<MeshFilter>();

        generate = GUI.changed || mfs == null || mfs.Length == 0;
        Generate();
        if (!generate) {
            for (int i = 0; i < mfs.Length; i++) {
                if (mfs[i].sharedMesh == null) {
                    generate = true;
                    break;
                }
            }
        }
    }

    void OnDestroy() {
        CleanUp();
    }

    void CleanUp() {
        // clear all previous objects
        var mfs = GetComponentsInChildren<MeshFilter>();
        for (int i = 0; i < mfs.Length; i++) {
            if (Application.isPlaying) {
                Destroy(mfs[i].sharedMesh);
                Destroy(mfs[i].gameObject);
            } else {
                DestroyImmediate(mfs[i].sharedMesh);
                DestroyImmediate(mfs[i].gameObject);
            }
        }
    }

    void Generate() {
        if (material == null || !material.HasProperty("_ShoreBlend")) return;

        if (material.GetFloat("_ShoreBlend") > 0.1f) {
            Camera.main.depthTextureMode |= DepthTextureMode.Depth;
        }

        if (!generate) return;
        generate = false;
        GenerateSquare();
    }

    float Encode(Vector3 v) {
        var uv0 = Mathf.Round((v.x + 5) * 10000f);
        var uv1 = Mathf.Round((v.z + 5) * 10000f) / 100000f;
        return uv0 + uv1;
    }

    void BakeMesh(List<Vector3> verts, List<int> inds, float rotation = 0f) {
        var uvs = new List<Vector2>(inds.Count);
        var splitIndices = new List<int>(inds.Count);
        var splitVertices = new List<Vector3>(inds.Count);

        for (int i = 0; i < inds.Count; i += 3) {
            splitIndices.Add(i % maxVerts);
            splitIndices.Add((i + 1) % maxVerts);
            splitIndices.Add((i + 2) % maxVerts);

            var v0 = verts[inds[i]];
            var v1 = verts[inds[i + 1]];
            var v2 = verts[inds[i + 2]];

            splitVertices.Add(v0);
            splitVertices.Add(v1);
            splitVertices.Add(v2);

            var uv = new Vector2();
            uv.x = Encode(v0 - v1);
            uv.y = Encode(v0 - v2);
            uvs.Add(uv);

            uv.x = Encode(v1 - v2);
            uv.y = Encode(v1 - v0);
            uvs.Add(uv);

            uv.x = Encode(v2 - v0);
            uv.y = Encode(v2 - v1);
            uvs.Add(uv);
        }

        CleanUp();

        int numGO = Mathf.CeilToInt(splitVertices.Count / (float)maxVerts);
        for (int i = 0, pos = 0; i < numGO; i++, pos += maxVerts) {
            var go = new GameObject("WaterChunk");
            if (gameObject != null) go.layer = gameObject.layer;
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(0, rotation, 0);
            go.transform.localScale = Vector3.one;
            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = material;
            mr.receiveShadows = false;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            var mesh = new Mesh();
            mesh.name = "WaterChunk";

            var len = i == numGO - 1 ? splitVertices.Count - pos : maxVerts;

            mesh.SetVertices(splitVertices.GetRange(pos, len));
            mesh.SetTriangles(splitIndices.GetRange(pos, len), 0);
            mesh.SetUVs(0, uvs.GetRange(pos, len));
            mesh.hideFlags = HideFlags.HideAndDontSave;
            mf.mesh = mesh;
            go.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    void Add(List<Vector3> verts, Vector3 toAdd, float delta) {
        var n = UnityEngine.Random.insideUnitCircle * delta / 4f;
        toAdd.x += n.x;
        toAdd.z += n.y;
        verts.Add(toAdd);
    }

    void GenerateSquare() {
        var verts = new List<Vector3>();
        var inds = new List<int>();
        var numVertsX = sizeX*2;
        var numVertsZ = sizeZ*2;
        var deltaX = Vector3.right * sin60;
        var vO = new Vector3(-sizeX * sin60, 0, -sizeZ * sin60);

        for (int j = 0; j < numVertsZ + 1; j++) {
            bool reverse = j % 2 != 0;
            var v = vO + Vector3.forward * j * sin60;
            int cols = numVertsX + (reverse ? 2 : 1);
            for (int i = 0; i < cols; i++) {
                Add(verts, v, sin60);
                if (reverse && (i == 0 || i == cols - 2)) {
                    v += deltaX / 2f;
                } else {
                    v += deltaX;
                }
            }
        }
        int iCur = 0;
        for (int j = 0; j < numVertsZ; j++) {
            bool reverse = j % 2 != 0;
            int ofs = numVertsX + (reverse ? 2 : 1); 
            int cols = numVertsX + (reverse ? 0 : 0);

            int iForw = iCur + ofs;

            for (int i = 0; i < cols; i++) {
                int iRight = iCur + 1;
                int iForwRight = iForw + 1;

                inds.Add(iCur);
                if (reverse) {
                    inds.Add(iForw);
                    inds.Add(iRight);
                    inds.Add(iForw);
                    inds.Add(iForwRight);
                    inds.Add(iRight);
                } else {
                    inds.Add(iForwRight);
                    inds.Add(iRight);
                    inds.Add(iCur);
                    inds.Add(iForw);
                    inds.Add(iForwRight);
                }
                iCur = iRight;
                iForw = iForwRight;
            }
            inds.Add(iCur);
            if (reverse) {
                inds.Add(iForw);
                inds.Add(iCur + 1);
                iCur += 2;
            } else {
                inds.Add(iForw);
                inds.Add(iForw + 1);
                iCur++;
            }
        }
        BakeMesh(verts, inds);
    }
}

