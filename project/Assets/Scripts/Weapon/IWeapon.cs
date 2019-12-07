using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon {

	/// <summary>
	/// Fires the <c>IWeapon</c>
	/// </summary>
	void Fire();

	/// <summary>
	/// Returns the available ammuniation of the <c>IWeapon</c>
	/// </summary>
	/// <returns></returns>
	int GetAmmo();

	/// <summary>
	/// Sets the ammunition of the <c>IWeapon</c> to the specified value
	/// </summary>
	/// <param name="ammo"></param>
	void SetAmmo(int ammo);


    /// <summary>
    /// Gets the audio component of the <c>IWeapon</c>
    /// </summary>
    /// <returns></returns>
    WeaponAudioComponent GetAudioComponent();

	/// <summary>
	/// Gets the <c>Person</c>, which is holding this <c>IWeapon</c> or <c>null</c>, if its on the ground
	/// </summary>
	/// <returns></returns>
	Person GetPerson();

	/// <summary>
	/// Sets the given <c>Person</c> as the owner of this <c>IWeapon</c>
	/// </summary>
	/// <param name="person"></param>
	void SetPerson(Person person);

	/// <summary>
	/// Gets the velocity of the weapon's projectile
	/// </summary>
	float GetProjectileVelocity();

    /// <summary>
    /// Returns the actual position of the weapon. 
    /// </summary>
    /// <returns></returns>
    Vector2 getActualPosition();

    /// <summary>
    /// Sets the animator on floor flag of the <c>IWeapon</c>
    /// </summary>
    /// <param name="active"></param>
    void SetAnimatorOnFloorFlag(bool active);

    /// <summary>
    /// Sets the sorting layer of the texture of the <c>IWeapon</c>
    /// </summary>
    /// <param name="sortinglayerConstant"></param>
    void SetSortingLayerOfTextureOfWeapon(string sortinglayerConstant);
}
