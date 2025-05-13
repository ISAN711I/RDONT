using UnityEngine;

public enum ItemType
{
    Perk
}

[CreateAssetMenu(fileName ="New Item",menuName ="Item")]
public class ItemData : ScriptableObject
{

    public Sprite Display;
    public ItemType type = ItemType.Perk;

    public string Name;
    public int Item_ID;
    [TextArea(15, 20)]
    public string description;

    public float damage;
    public float damage_mult;
    public float true_damage_mult;

    public float hp;
    public float hp_mult;
    public int revives;
    public float regen;

    public float speed;
    public float speed_mult;

    public float attack_speed;
    public float attack_speed_mult;

    public float ability_charge;


    public float shot_angle;
    public float true_angle_mult;
    public int num_shots;

    public float shot_speed;
    public float true_shot_speed_mult;


    public splitter mysplitter;

}
