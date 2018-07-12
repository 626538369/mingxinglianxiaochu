using System.Collections;

/// <summary>
/// ���� SDK �������õķ���״̬��
/// </summary>
public class UCStatusCode
{

	// ���óɹ�
	public const int SUCCESS = 0;

	// ����ʧ��
	public const int FAIL = -2;

	// û�г�ʼ��������ִ��
	public const int NO_INIT = -10;

	// û�е�¼������ִ��
	public const int NO_LOGIN = -11;

	// ��ⲻ������������ִ��
	public const int NO_NETWORK = -12;

	// ��ʼ��ʧ��
	public const int INIT_FAIL = -100;

	// ��¼ʧ��
	public const int LOGIN_FAIL = -200;


	// ��¼ʧ��,��Ҫ�����ڲ�����ܽ���
	public const int LOGIN_ALPHA_CODE_MISSING = -201;


	// ��Ϸ�ʻ���������µ�¼ʧ��
	public const int LOGIN_GAME_USER_AUTH_FAIL = -201;

	// ����ԭ������Ϸ�ʻ���¼ʧ��
	public const int LOGIN_GAME_USER_NETWORK_FAIL = -202;

	// ����ԭ���µ���Ϸ�ʻ���¼ʧ��
	public const int LOGIN_GAME_USER_OTHER_FAIL = -203;


	// ��ȡ���ѹ�ϵʧ��
	public const int GETFRINDS_FAIL = -300;

	// ��ȡ�û��Ƿ��Աʱʧ��
	public const int VIP_FAIL = -400;

	// ��ȡ�û���Ա��Ȩ��Ϣʱʧ��
	public const int VIPINFO_FAIL = -401;


	// �û��˳���ֵ����
	public const int PAY_USER_EXIT = -500;


	// �û��˳���¼����
	public const int LOGIN_EXIT = -600;


	// SDK���潫Ҫ��ʾ
	public const int SDK_OPEN = -700;

	// SDK���潫Ҫ�رգ����ص���Ϸ����
	public const int SDK_CLOSE = -701;


	// �ο�״̬
	public const int GUEST = -800;


	// uc�˻���¼״̬
	public const int UC_ACCOUNT = -801;


	// �˳��ο����漤���ҳ��ص�״̬��
	public const int BIND_EXIT = -900;

}
