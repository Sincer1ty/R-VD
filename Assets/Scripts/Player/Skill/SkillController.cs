using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines.SkillDefines;
using UnityEditor;

public class SkillController : MonoBehaviour
{
    // ��ų ��ųʸ�
    private Dictionary<SkillIndex, SkillBase> skillDictionary = new Dictionary<SkillIndex, SkillBase>();

    // ��ų ����
    public SkillBase CurrentSkill { get; private set; }
    public SkillBase PrevSkill { get; private set; }

    private bool isThereSkill = false;



    private void Awake()
    {
        isThereSkill = false;
        CurrentSkill = PrevSkill = null;
    }

    /// <summary>
    /// ��ų�� ����Ѵ�.
    /// </summary>
    /// <returns>����� ���������� true, �ƴϸ� false</returns>
    public bool AddSkillBase(SkillIndex _index, SkillBase _skillBase)
    {
        if (skillDictionary.ContainsKey(_index) == true)
            return false;

        _skillBase.SetController(this);
        _skillBase.OnInit();
        skillDictionary[_index] = _skillBase;
        return true;
    }

    /// <summary>
    /// ��ų�� ��� ���� �Ѵ�.
    /// </summary>
    public void RemoveSkillBase(SkillIndex _index)
    {
        if (skillDictionary.ContainsKey(_index) == false)
            return;

        skillDictionary[_index].SetController(null);
        skillDictionary.Remove(_index);
        return;
    }

    public bool SelectSkill(SkillIndex skillindex)
    {
        if(isThereSkill)
        {
            PrevSkill = CurrentSkill;
            PrevSkill.OnDetach();
            isThereSkill = false;
        }

        if (skillDictionary.ContainsKey(skillindex) == true)
        {
            CurrentSkill = skillDictionary[skillindex];
            CurrentSkill.OnAttach();
            isThereSkill = true;
        }

        return true;
    }


    private void FixedUpdate()
    {
        if (isThereSkill == false)
            return;

        CurrentSkill.OnFixedUpdate();
    }

    private void Update()
    {
        if (isThereSkill == false)
            return;

        CurrentSkill.OnUpdate();
    }

    private void LateUpdate()
    {
        if (isThereSkill == false)
            return;

        CurrentSkill.OnLateUpdate();
    }
}
