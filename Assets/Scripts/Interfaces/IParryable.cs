using System;
using UnityEngine;

public interface IParryable
{
   float GetParryOverallTiming();
   float GetParryCoolDownTiming();
   bool isThisActionParryableNow();
    
   ParryInfo GetParryInfo();
   void OnParryFailed();   
   void OnParrySuccess();
   event Action OnParryObjectHit;   
   ParryController GetParryController();
}
