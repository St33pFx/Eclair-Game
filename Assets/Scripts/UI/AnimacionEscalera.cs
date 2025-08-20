using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class PauseMenuDOTweenSimple : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] GameObject pauseRoot;           // Panel raíz del menú de pausa (tu "Pause")
    [SerializeField] CanvasGroup rootCG;             // CanvasGroup del root
    [SerializeField] RectTransform leftMenuRoot;     // El contenedor que tiene el Vertical Layout Group
    [SerializeField] Selectable firstSelected;       // Botón inicial (Resume)
    [SerializeField] RectTransform logo;             // Opcional

    [Header("Items (orden)")]
    [SerializeField] List<RectTransform> items;      // Btn_Resume, Btn_Option, Btn_Menu, Btn_Quit

    [Header("Anim")]
    [SerializeField] float slideDist = 140f;
    [SerializeField] float inDuration = 0.45f;
    [SerializeField] float outDuration = 0.30f;
    [SerializeField] float stagger = 0.08f;
    [SerializeField] Ease easeIn = Ease.OutCubic;
    [SerializeField] Ease easeOut = Ease.InSine;

    Sequence seqIn, seqOut;
    bool built, isPaused;

    Vector2 leftMenuStartPos;
    Vector3 logoScale;
    CanvasGroup[] itemCGs;

    void Awake()
    {
        // Mantén el panel activo y oculto (mejor que desactivarlo)
        if (pauseRoot) pauseRoot.SetActive(true);
        if (rootCG) { rootCG.alpha = 0f; rootCG.interactable = false; rootCG.blocksRaycasts = false; }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    void EnsureBuilt()
    {
        if (built) return;

        // Asegura layout antes de cachear
        if (leftMenuRoot) LayoutRebuilder.ForceRebuildLayoutImmediate(leftMenuRoot);

        leftMenuStartPos = leftMenuRoot.anchoredPosition;
        if (logo) logoScale = logo.localScale;

        // Asegura CanvasGroup por item para hacer fade
        itemCGs = new CanvasGroup[items.Count];
        for (int i = 0; i < items.Count; i++)
        {
            var cg = items[i].GetComponent<CanvasGroup>();
            if (!cg) cg = items[i].gameObject.AddComponent<CanvasGroup>();
            itemCGs[i] = cg;
        }

        BuildSequences();
        built = true;
    }

    void BuildSequences()
    {
        // Estado inicial para entrada
        leftMenuRoot.anchoredPosition = leftMenuStartPos - new Vector2(0, slideDist);
        if (logo)
        {
            logo.localScale = logoScale * 0.97f;
        }
        for (int i = 0; i < items.Count; i++)
        {
            itemCGs[i].alpha = 0f;
            items[i].localScale = Vector3.one * 0.95f;
        }

        // ========== ENTRADA ==========
        seqIn = DOTween.Sequence().SetUpdate(true).SetAutoKill(false).Pause();

        // Fade del root
        seqIn.Append(rootCG.DOFade(1f, inDuration * 0.6f));

        // Slide del contenedor completo
        seqIn.Join(leftMenuRoot.DOAnchorPos(leftMenuStartPos, inDuration).SetEase(easeIn));

        // Logo opcional (pequeño punch)
        if (logo)
            seqIn.Join(logo.DOScale(logoScale * 1.03f, inDuration * 0.45f).From(logoScale * 0.97f));

        // Items en escalera: fade + pop
        for (int i = 0; i < items.Count; i++)
        {
            float delay = (i + (logo ? 1 : 0)) * stagger;
            seqIn.Insert(delay, itemCGs[i].DOFade(1f, inDuration * 0.5f));
            seqIn.Insert(delay, items[i].DOScale(1.03f, inDuration * 0.45f).From(0.95f));
        }

        seqIn.OnStart(() =>
        {
            rootCG.interactable = false;
            rootCG.blocksRaycasts = false;
        });

        seqIn.OnComplete(() =>
        {
            rootCG.interactable = true;
            rootCG.blocksRaycasts = true;

            // Selección inicial
            if (firstSelected)
            {
                EventSystem.current.SetSelectedGameObject(null);
                firstSelected.Select();
            }
        });

        // ========== SALIDA ==========
        seqOut = DOTween.Sequence().SetUpdate(true).SetAutoKill(false).Pause();

        seqOut.OnStart(() =>
        {
            rootCG.interactable = false;
            rootCG.blocksRaycasts = false;
        });

        // Items apagan en reversa
        for (int i = items.Count - 1; i >= 0; i--)
        {
            float delay = (items.Count - 1 - i) * stagger;
            seqOut.Insert(delay, itemCGs[i].DOFade(0f, outDuration * 0.6f));
            seqOut.Insert(delay, items[i].DOScale(0.98f, outDuration * 0.5f).From(1f));
        }

        // Logo desinfla y contenedor baja
        if (logo)
            seqOut.Insert(0f, logo.DOScale(logoScale * 0.97f, outDuration * 0.5f).From(logoScale));
        seqOut.Join(leftMenuRoot.DOAnchorPos(leftMenuStartPos - new Vector2(0, slideDist), outDuration).SetEase(easeOut));

        // Fade root
        seqOut.Join(rootCG.DOFade(0f, outDuration * 0.8f));

        // Dejar todo listo para la próxima (estado inicial)
        seqOut.OnComplete(() =>
        {
            leftMenuRoot.anchoredPosition = leftMenuStartPos - new Vector2(0, slideDist);
            if (logo) logo.localScale = logoScale * 0.97f;
            for (int i = 0; i < items.Count; i++)
            {
                itemCGs[i].alpha = 0f;
                items[i].localScale = Vector3.one * 0.95f;
            }
            // Si prefieres desactivar completamente:
            // pauseRoot.SetActive(false);
        });
    }

    void KillAllTweens()
    {
        // Mata secuencias si existen
        if (seqIn != null) { seqIn.Kill(false); seqIn = null; }
        if (seqOut != null) { seqOut.Kill(false); seqOut = null; }

        // Mata tweens sueltos que pudieran quedar sobre los mismos targets
        if (rootCG) DOTween.Kill(rootCG, false);
        if (leftMenuRoot) DOTween.Kill(leftMenuRoot, false);
        if (logo) DOTween.Kill(logo, false);

        if (items != null)
        {
            foreach (var rt in items)
            {
                if (!rt) continue;
                DOTween.Kill(rt, false);
                var cg = rt.GetComponent<CanvasGroup>();
                if (cg) DOTween.Kill(cg, false);
            }
        }
    }

    void ResetToEntryStart()
    {
        // Posición del contenedor (offscreen)
        leftMenuRoot.anchoredPosition = leftMenuStartPos - new Vector2(0, slideDist);

        // Logo
        if (logo) logo.localScale = logoScale * 0.97f;

        // Items
        for (int i = 0; i < items.Count; i++)
        {
            if (itemCGs != null && i < itemCGs.Length && itemCGs[i])
                itemCGs[i].alpha = 0f;
            items[i].localScale = Vector3.one * 0.95f;
        }

        // Root
        rootCG.alpha = 0f;
        rootCG.interactable = false;
        rootCG.blocksRaycasts = false;
    }



    // ==== API pública ====
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused) ShowPause();
        else HidePause();
    }

    public void ShowPause()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;

        pauseRoot.SetActive(true);
        EnsureBuilt();

        // Garantiza que no haya nada colgando de una apertura anterior
        KillAllTweens();
        BuildSequences();          // reconstruye seqIn/seqOut
        ResetToEntryStart();       // fuerza estado inicial

        // Reproduce entrada
        seqIn.PlayForward();

        // Foco al primer botón al final lo hace OnComplete de seqIn (como ya tienes)
    }

    public void HidePause()
    {
        // Garantiza que la entrada no siga viva
        if (seqIn != null) seqIn.Kill(false);

        // Reconstruye solo la salida por si acaso
        KillAllTweens();
        BuildSequences();          // crea de nuevo seqOut también

        // Coloca el root visible para poder hacer fade out limpio
        rootCG.alpha = 1f;
        rootCG.interactable = false;
        rootCG.blocksRaycasts = false;
        leftMenuRoot.anchoredPosition = leftMenuStartPos;

        // Reproduce salida
        seqOut.PlayForward();

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }


}
