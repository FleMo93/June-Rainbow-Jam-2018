using UnityEngine;

public class scr_Tree : MonoBehaviour, i_Interactable, i_Damageable {
    scr_Stats stats;

    [SerializeField]
    private ParticleSystem _ParticleOnDamage;
    [SerializeField]
    private GameObject _MainModel;
    [SerializeField]
    private GameObject _Stub;
    [SerializeField]
    private GameObject _FelledBody;
    [SerializeField]
    private float _FallSpeed = 25f;
    [SerializeField]
    private float _FallSpeedAcceleration = 1f;
    [SerializeField]
    private float _DisappearSpeed = 0.9f;

    Vector3 fallDirection = Vector3.zero;

    void Start () {
        stats = gameObject.GetComponent<scr_Stats>();
        _Stub.SetActive(false);
        _FelledBody.SetActive(false);
	}

    public void Damage(int damage, GameObject sender) 
    {
        stats.Health -= damage;
        _ParticleOnDamage.Play();

        if(stats.Health <= 0)
        {
            _Stub.SetActive(true);
            _FelledBody.SetActive(true);
            _MainModel.SetActive(false);

            Debug.Log(scr_Tilemap.Get.GetDirectionFromTo(sender.transform.position, this.gameObject.transform.position));
            switch(scr_Tilemap.Get.GetDirectionFromTo(sender.transform.position, this.gameObject.transform.position)) 
            {
                case scr_Stats.Directions.Up:
                    fallDirection = transform.InverseTransformDirection(new Vector3(1, 0, 0));
                    break;

                case scr_Stats.Directions.Right:
                    fallDirection = transform.InverseTransformDirection(new Vector3(0, 0, -1));
                    break;

                case scr_Stats.Directions.Down:
                    fallDirection = transform.InverseTransformDirection(new Vector3(-1, 0, 0));
                    break;

                case scr_Stats.Directions.Left:
                    fallDirection = transform.InverseTransformDirection(new Vector3(0, 0, 1));
                    break;
            }
        }
    }

    public scr_Interactable_Result Interact(GameObject trigger, scr_Stats.ObjectType itemInInventory)
    {
        bool successfull = itemInInventory == scr_Stats.ObjectType.Axe && stats.Health > 0;

        return new scr_Interactable_Result(scr_Stats.Interaction.ChopTree, successfull, damagable: this);
    }

    float rotationDone = 0;
    float fallSpeed = 0;
    void Update()
    {
        if(fallSpeed == 0)
        {
            fallSpeed = _FallSpeed;
        }

        fallSpeed += _FallSpeedAcceleration * Time.deltaTime;

        if(stats.Health <= 0)
        {
            if (rotationDone <= 90)
            {
                float rotation = fallSpeed * Time.deltaTime;
                rotationDone += rotation;
                _FelledBody.transform.Rotate(fallDirection * rotation);
            }
            else if(_FelledBody.transform.localScale.x > 0)
            {
                _FelledBody.transform.localScale = new Vector3(
                    _FelledBody.transform.localScale.x - _DisappearSpeed * Time.deltaTime,
                    _FelledBody.transform.localScale.y - _DisappearSpeed * Time.deltaTime,
                    _FelledBody.transform.localScale.z - _DisappearSpeed * Time.deltaTime
                    );

                if(_FelledBody.transform.localScale.x <= 0) 
                {
                    _FelledBody.transform.localScale = Vector3.zero;
                } 
            }
        }

    }
}
