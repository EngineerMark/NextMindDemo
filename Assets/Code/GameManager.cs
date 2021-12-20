using NextMind;
using NextMind.Calibration;
using NextMind.Devices;
using NextMind.NeuroTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NeuroManager neuroManager;
    public CalibrationManager calibrationManager;
    public GameObject calibrationSystem;
    public GameObject calibrationSystemCore;
    public GameObject demoGameButton;
    public GameObject demoGame;
    public TMPro.TMP_Text calibrationStepsText;

    public IEnumerator Start(){
        Application.targetFrameRate = 90;
        calibrationStepsText.gameObject.SetActive(false);
        calibrationSystem.SetActive(false);
        demoGameButton.SetActive(false);
        demoGame.SetActive(false);
        if(!NeuroManager.Instance.SimulateDevice){
            yield return null;
            yield return new WaitUntil(()=>NeuroManager.Instance.IsReady());
            calibrationSystem.SetActive(true);
            yield return null;
            calibrationManager.StartCalibration();
            calibrationManager.onCalibrationResultsAvailable.AddListener(OnReceivedResults);
        }else{
            calibrationSystem.SetActive(true);
            calibrationSystemCore.SetActive(false);
            calibrationManager.gameObject.SetActive(false);
            OnReceivedResults(null, CalibrationResults.CalibrationGrade.A);
        }
    }

    private void OnReceivedResults(Device device, CalibrationResults.CalibrationGrade grade)
    {
        calibrationStepsText.text = "Received results for "+(device!=null?device.Name:"")+" with a grade of "+grade;
        calibrationStepsText.gameObject.SetActive(true);
        demoGameButton.SetActive(true);
    }

    public void PlayGame(){
        demoGameButton.SetActive(false);
        calibrationSystem.SetActive(false);
        demoGame.SetActive(true);
        demoGame.GetComponent<MatchManager>().Reset();
    }
}
