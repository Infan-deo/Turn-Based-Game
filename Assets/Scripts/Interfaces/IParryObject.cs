using UnityEngine;

public interface IParryObject
{
   void OnSuccessfullParry(ParryController parryController);
   void OnUnsuccessfullParry(ParryController parryController);
}
