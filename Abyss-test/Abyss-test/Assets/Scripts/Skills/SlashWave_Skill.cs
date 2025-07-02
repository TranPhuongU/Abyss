using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashWave_Skill : Skill
{
    [SerializeField] private GameObject slashWavePrefab1;
    [SerializeField] private GameObject slashWavePrefab2;
    [SerializeField] private GameObject slashWavePrefab3;

    [SerializeField] private float slashWaveSpeed;
    [SerializeField] private float slashWaveDamageRate;

    [Header("SlashWave info")]
    public bool canSlashWave = false;
    public float canSlashWaveTime;
    public float canSlashWaveTimer;
    public int defaultAmountOfSlashWave;
    public int amountOfSlashWave;

    protected override void Start()
    {
        base.Start();
        canSlashWaveTimer = canSlashWaveTime;
    }

    protected override void Update()
    {
        base.Update();
        CanSlashWave();
    }
    private void CanSlashWave()
    {
        if (canSlashWave)
        {
            canSlashWaveTimer -= Time.deltaTime;

            if (canSlashWaveTimer <= 0)
            {
                canSlashWaveTimer = canSlashWaveTime;
                canSlashWave = false;
            }
        }
    }
    public void CreateSlashWave3(Transform _attackCheck)
    {
        GameObject newSlashWave = Instantiate(slashWavePrefab3, _attackCheck.position, Quaternion.identity);

        newSlashWave.GetComponent<SlashWave_Controller>().SetupSlashWave(slashWaveSpeed * PlayerManager.instance.player.facingDir, PlayerManager.instance.player.stats, slashWaveDamageRate);      
    }

    public void CreateSlashWave1(Transform _attackCheck)
    {
        GameObject newSlashWave = Instantiate(slashWavePrefab1, _attackCheck.position, Quaternion.identity);

        newSlashWave.GetComponent<SlashWave_Controller>().SetupSlashWave(slashWaveSpeed * PlayerManager.instance.player.facingDir, PlayerManager.instance.player.stats, slashWaveDamageRate);
    }
    public void CreateSlashWave2(Transform _attackCheck)
    {
        GameObject newSlashWave = Instantiate(slashWavePrefab2, _attackCheck.position, Quaternion.identity);

        newSlashWave.GetComponent<SlashWave_Controller>().SetupSlashWave(slashWaveSpeed * PlayerManager.instance.player.facingDir, PlayerManager.instance.player.stats, slashWaveDamageRate);
    }

}
