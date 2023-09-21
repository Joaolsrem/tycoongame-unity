using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int speed;
    public int speedRotation;

    public int Money;
    public Slider BarraMinerio;
    public GameObject MenuMineracao;
    public Text TextoMinerio;
    public float TimeCount;
    private bool playerColidiu;
    public int MinerioQNTD;
    public int MinerioPlayer;
    public float TempoSemClicar;
    public bool JogadorClicou;

    public GameObject MenuLoja;
    public Text TextoMoney;
    public bool comprouBoost;
    public Text Tempoboost;

    public GameObject NPCMinerador;
    public float TimeMiner, TimeMinerWithUpgrade;
    public bool comprouMinerador;

    public GameObject PicaretaNPC;
    public GameObject ParteCima;
    public bool comprouUpgrade1, comprouUpgrade2, comprouUpgrade3, algumUpgrade;
    public MeshRenderer PicNPCcolor;
    public Image IconUpgrade1;
    public Image IconUpgrade2;
    public Image IconUpgrade3;
    public GameObject BotaoUpgrade1;
    public GameObject BotaoUpgrade2;
    public GameObject BotaoUpgrade3;

    public GameObject spawnerPoint;
    public GameObject bulletPrefab;
    public GameObject bullet;
    public int speedBullet;
    public bool podeAtirar = true;

    void Start()
    {
        BotaoUpgrade1.SetActive(false);
        BotaoUpgrade2.SetActive(false);
        BotaoUpgrade3.SetActive(false);
        MenuMineracao.SetActive(false);
        MenuLoja.SetActive(false);
        NPCMinerador.SetActive(false);
        IconUpgrade1.enabled = false;
        IconUpgrade2.enabled = false;
        IconUpgrade3.enabled = false;
        Tempoboost.enabled = false;
        BarraMinerio.maxValue = 5;
        PicNPCcolor = ParteCima.GetComponent<MeshRenderer>();
        bulletPrefab.transform.Translate(transform.forward * Time.deltaTime * speedBullet);
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * speedRotation * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * speedRotation * Time.deltaTime);
        }

        if (podeAtirar && Input.GetKey(KeyCode.Space))
        {
            bullet = bulletPrefab;
            StartCoroutine(balaSpawner());
        }

        if (playerColidiu && Input.GetKeyDown(KeyCode.E))
        {
            BarraMinerio.value += 0.10f;
            TempoSemClicar = 0f;
            JogadorClicou = true;
        }

        if (playerColidiu && Input.GetKeyUp(KeyCode.E))
        {
            JogadorClicou = false;
        }

        if (!JogadorClicou && playerColidiu)
        {
            TempoSemClicar += Time.deltaTime;

            if (TempoSemClicar >= 1f)
            {
                DiminuirBarra();
                Debug.Log("Jogador ficou 1 segundo sem clicar!");
            }
        }

        if (BarraMinerio.value == 5f)
        {
            MinerioPlayer += MinerioQNTD;
            TextoMinerio.text = MinerioPlayer.ToString();
            BarraMinerio.value = 0;
        }
        TextoMoney.text = Money.ToString();
        TextoMinerio.text = MinerioPlayer.ToString();

        // SISTEMA DA LOJA

        if (comprouBoost)
        {
            TimeCount += Time.deltaTime;
            if (TimeCount < 30f)
            {
                MinerioQNTD = 10;
                Tempoboost.enabled = true;
                Tempoboost.text = "Tempo Boost: " + TimeCount.ToString();
            } else if (TimeCount >= 20f)
            {
                TimeCount = 0f;
                Tempoboost.enabled = false;
                MinerioQNTD = 2;
                Debug.Log("Tempo do boost terminou");
                comprouBoost = false;
            }
        }

        //SISTEMA NPC MINERADOR
        if (comprouMinerador)
        {
            TimeMiner += Time.deltaTime;
            TimeMinerWithUpgrade += Time.deltaTime;
            if (TimeMiner >= 5 && algumUpgrade == false)
            {
                TimeMiner = 0;
                MinerioPlayer += 2;
            }
            if (TimeMinerWithUpgrade >= 5 && algumUpgrade == true)
            {
                TimeMiner = 0;
                TimeMinerWithUpgrade = 0;
                if (comprouUpgrade1)
                {
                    MinerioPlayer += 4;
                    PicNPCcolor.material.color = new Color(147, 147, 147);
                }
                if (comprouUpgrade2)
                {
                    MinerioPlayer += 6;
                    PicNPCcolor.material.color = new Color(255, 217, 0);
                }
                if (comprouUpgrade3)
                {
                    MinerioPlayer += 20;
                    PicNPCcolor.material.color = new Color(0, 120, 255, 255);
                }
            }
        }

    }

    public void DiminuirBarra()
    {
        if (TempoSemClicar >= 1f)
        {
            Debug.Log("DiminuirBarra Sendo Executada");
            TempoSemClicar--;
            BarraMinerio.value -= 0.20f;
        }
    }

    public void OnTriggerEnter(Collider colisaoMinerio)
    {
        if (colisaoMinerio.gameObject.tag == "Minerio")
        {
            MenuMineracao.SetActive(true);
            playerColidiu = true;
        }

        if (colisaoMinerio.gameObject.tag == "MenuLoja")
        {
            MenuLoja.SetActive(true);
        }
    }
    public void OnTriggerExit(Collider colisaoMinerio)
    {
        if (colisaoMinerio.gameObject.tag == "Minerio")
        {
            MenuMineracao.SetActive(false);
            playerColidiu = false;
        }

        if (colisaoMinerio.gameObject.tag == "MenuLoja")
        {
            MenuLoja.SetActive(false);
        }
    }

    public void VendasMinerios()
    {
        if (MinerioPlayer > 0)
        {
            Money += MinerioPlayer * MinerioQNTD;
            MinerioPlayer -= MinerioPlayer;
        }
        if (MinerioPlayer <= 0)
        {
            Debug.Log("Voce nao tem minerio para vender!");
        }
    }

    public void comprar2XBoost()
    {
        if (Money >= 10)
        {
            Money -= 10;
            comprouBoost = true;
        }
        else
        {
            comprouBoost = false;
        }
    }

    public void comprarMinerador()
    {
        if (Money >= 100)
        {
            Money -= 100;
            NPCMinerador.SetActive(true);
            IconUpgrade1.enabled = true;
            BotaoUpgrade1.SetActive(true);
            comprouMinerador = true;
        }
    }

    public void comprarUpgrade1()
    {
        if (Money >= 110)
        {
            Money -= 110;
            IconUpgrade1.enabled = false;
            IconUpgrade2.enabled = true;
            BotaoUpgrade1.SetActive(false);
            BotaoUpgrade2.SetActive(true);
            comprouUpgrade1 = true;
            algumUpgrade = true;
        }
    }

    public void comprarUpgrade2()
    {
        if (Money >= 250)
        {
            Money -= 250;
            comprouUpgrade1 = false;
            comprouUpgrade2 = true;
            BotaoUpgrade2.SetActive(false);
            BotaoUpgrade3.SetActive(true);
            IconUpgrade2.enabled = false;
            IconUpgrade3.enabled = true;
        }
    }

    public void comprouupgrade3()
    {
        if (comprouUpgrade3 == false && Money >= 500)
        {
            Money -= 500;
            comprouUpgrade2 = false;
            comprouUpgrade3 = true;
            IconUpgrade3.enabled = true;
        }
    }

    IEnumerator balaSpawner()
    {
        podeAtirar = false;
        bullet = Instantiate(bulletPrefab, spawnerPoint.transform.position, spawnerPoint.transform.rotation);
        yield return new WaitForSeconds(0.6f);
        podeAtirar = true;
    }

}
