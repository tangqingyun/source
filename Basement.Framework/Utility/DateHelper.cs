using System;
using System.Collections.Generic;
using System.Text;

namespace Basement.Framework.Utility
{
    #region = ���ڴ��� =
    /// <summary>
    /// ���ڴ���
    /// </summary>
    /// <example></example>
    public class DateHelper
    {
        private const ushort START_YEAR = 1901;
        private const ushort END_YEAR = 2050;
        private string[] ConstellationName ={  
												"������", "��ţ��", "˫����",  
												"��з��", "ʨ����", "��Ů��",  
												"�����", "��Ы��", "������",  
												"Ħ����", "ˮƿ��", "˫����"
											};
        private string[] LunarHolDayName =  {  
												"С��", "��", "����", "��ˮ",  
												"����", "����", "����", "����",  
												"����", "С��", "â��", "����",  
												"С��", "����", "����", "����",  
												"��¶", "���", "��¶", "˪��",  
												"����", "Сѩ", "��ѩ", "����"
											};

        //����gLunarDay��������1901�굽2100��ÿ���е���������Ϣ��  
        //����ÿ��ֻ����29��30�죬һ����12����13����������λ��ʾ����ӦλΪ1��30�죬����Ϊ29��  
        private int[] gLunarMonthDay = {  
										   //��������ֻ��1901.1.1 --2050.12.31  
										   0x4ae0, 0xa570, 0x5268, 0xd260, 0xd950, 0x6aa8, 0x56a0, 0x9ad0, 0x4ae8, 0x4ae0, //1910  
										   0xa4d8, 0xa4d0, 0xd250, 0xd548, 0xb550, 0x56a0, 0x96d0, 0x95b0, 0x49b8, 0x49b0, //1920  
										   0xa4b0, 0xb258, 0x6a50, 0x6d40, 0xada8, 0x2b60, 0x9570, 0x4978, 0x4970, 0x64b0, //1930  
										   0xd4a0, 0xea50, 0x6d48, 0x5ad0, 0x2b60, 0x9370, 0x92e0, 0xc968, 0xc950, 0xd4a0, //1940  
										   0xda50, 0xb550, 0x56a0, 0xaad8, 0x25d0, 0x92d0, 0xc958, 0xa950, 0xb4a8, 0x6ca0, //1950  
										   0xb550, 0x55a8, 0x4da0, 0xa5b0, 0x52b8, 0x52b0, 0xa950, 0xe950, 0x6aa0, 0xad50, //1960  
										   0xab50, 0x4b60, 0xa570, 0xa570, 0x5260, 0xe930, 0xd950, 0x5aa8, 0x56a0, 0x96d0, //1970  
										   0x4ae8, 0x4ad0, 0xa4d0, 0xd268, 0xd250, 0xd528, 0xb540, 0xb6a0, 0x96d0, 0x95b0, //1980  
										   0x49b0, 0xa4b8, 0xa4b0, 0xb258, 0x6a50, 0x6d40, 0xada0, 0xab60, 0x9370, 0x4978, //1990  
										   0x4970, 0x64b0, 0x6a50, 0xea50, 0x6b28, 0x5ac0, 0xab60, 0x9368, 0x92e0, 0xc960, //2000  
										   0xd4a8, 0xd4a0, 0xda50, 0x5aa8, 0x56a0, 0xaad8, 0x25d0, 0x92d0, 0xc958, 0xa950, //2010  
										   0xb4a0, 0xb550, 0xb550, 0x55a8, 0x4ba0, 0xa5b0, 0x52b8, 0x52b0, 0xa930, 0x74a8, //2020  
										   0x6aa0, 0xad50, 0x4da8, 0x4b60, 0x9570, 0xa4e0, 0xd260, 0xe930, 0xd530, 0x5aa0, //2030  
										   0x6b50, 0x96d0, 0x4ae8, 0x4ad0, 0xa4d0, 0xd258, 0xd250, 0xd520, 0xdaa0, 0xb5a0, //2040  
										   0x56d0, 0x4ad8, 0x49b0, 0xa4b8, 0xa4b0, 0xaa50, 0xb528, 0x6d20, 0xada0, 0x55b0}; //2050  

        //����gLanarMonth�������1901�굽2050�����µ��·ݣ���û����Ϊ0��ÿ�ֽڴ�����  
        byte[] gLunarMonth ={  
							   0x00, 0x50, 0x04, 0x00, 0x20, //1910  
							   0x60, 0x05, 0x00, 0x20, 0x70, //1920  
							   0x05, 0x00, 0x40, 0x02, 0x06, //1930  
							   0x00, 0x50, 0x03, 0x07, 0x00, //1940  
							   0x60, 0x04, 0x00, 0x20, 0x70, //1950  
							   0x05, 0x00, 0x30, 0x80, 0x06, //1960  
							   0x00, 0x40, 0x03, 0x07, 0x00, //1970  
							   0x50, 0x04, 0x08, 0x00, 0x60, //1980  
							   0x04, 0x0a, 0x00, 0x60, 0x05, //1990  
							   0x00, 0x30, 0x80, 0x05, 0x00, //2000  
							   0x40, 0x02, 0x07, 0x00, 0x50, //2010  
							   0x04, 0x09, 0x00, 0x60, 0x04, //2020  
							   0x00, 0x20, 0x60, 0x05, 0x00, //2030  
							   0x30, 0xb0, 0x06, 0x00, 0x50, //2040  
							   0x02, 0x07, 0x00, 0x50, 0x03}; //2050  

        //����gLanarHoliDay���ÿ��Ķ�ʮ�Ľ�����Ӧ����������  
        //ÿ��Ķ�ʮ�Ľ�����Ӧ���������ڼ����̶���ƽ���ֲ���ʮ��������  
        // 1�� 2�� 3�� 4�� 5�� 6��  
        //С�� �� ���� ��ˮ ���� ���� ���� ���� ���� С�� â�� ����  
        // 7�� 8�� 9�� 10�� 11�� 12��  
        //С�� ���� ���� ���� ��¶ ��� ��¶ ˪�� ���� Сѩ ��ѩ ����  
        //*********************************************************************************  
        // �������κ�ȷ������,����ֻ�ô��,Ҫ��ʡ�ռ�,����....  
        //**********************************************************************************}  
        //���ݸ�ʽ˵��:  
        //��1901��Ľ���Ϊ  
        // 1�� 2�� 3�� 4�� 5�� 6�� 7�� 8�� 9�� 10�� 11�� 12��  
        // 6, 21, 4, 19, 6, 21, 5, 21, 6,22, 6,22, 8, 23, 8, 24, 8, 24, 8, 24, 8, 23, 8, 22  
        // 9, 6, 11,4, 9, 6, 10,6, 9,7, 9,7, 7, 8, 7, 9, 7, 9, 7, 9, 7, 8, 7, 15  
        //�����һ������Ϊÿ�½�����Ӧ����,15��ȥÿ�µ�һ������,ÿ�µڶ���������ȥ15�õڶ���  
        // ����ÿ������������Ӧ���ݶ�С��16,ÿ����һ���ֽڴ��,��λ��ŵ�һ����������,��λ���  
        //�ڶ�������������,�ɵ��±�  
        byte[] gLunarHolDay ={  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1901  
								0x96, 0xA4, 0x96, 0x96, 0x97, 0x87, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1902  
								0x96, 0xA5, 0x87, 0x96, 0x87, 0x87, 0x79, 0x69, 0x69, 0x69, 0x78, 0x78, //1903  
								0x86, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x78, 0x87, //1904  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1905  
								0x96, 0xA4, 0x96, 0x96, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1906  
								0x96, 0xA5, 0x87, 0x96, 0x87, 0x87, 0x79, 0x69, 0x69, 0x69, 0x78, 0x78, //1907  
								0x86, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1908  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1909  
								0x96, 0xA4, 0x96, 0x96, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1910  
								0x96, 0xA5, 0x87, 0x96, 0x87, 0x87, 0x79, 0x69, 0x69, 0x69, 0x78, 0x78, //1911  
								0x86, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1912  
								0x95, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1913  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1914  
								0x96, 0xA5, 0x97, 0x96, 0x97, 0x87, 0x79, 0x79, 0x69, 0x69, 0x78, 0x78, //1915  
								0x96, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1916  
								0x95, 0xB4, 0x96, 0xA6, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x87, //1917  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x77, //1918  
								0x96, 0xA5, 0x97, 0x96, 0x97, 0x87, 0x79, 0x79, 0x69, 0x69, 0x78, 0x78, //1919  
								0x96, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1920  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x87, //1921  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x77, //1922  
								0x96, 0xA4, 0x96, 0x96, 0x97, 0x87, 0x79, 0x79, 0x69, 0x69, 0x78, 0x78, //1923  
								0x96, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1924  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x87, //1925  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1926  
								0x96, 0xA4, 0x96, 0x96, 0x97, 0x87, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1927  
								0x96, 0xA5, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1928  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1929  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1930  
								0x96, 0xA4, 0x96, 0x96, 0x97, 0x87, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1931  
								0x96, 0xA5, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1932  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1933  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1934  
								0x96, 0xA4, 0x96, 0x96, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1935  
								0x96, 0xA5, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1936  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1937  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1938  
								0x96, 0xA4, 0x96, 0x96, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1939  
								0x96, 0xA5, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1940  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1941  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1942  
								0x96, 0xA4, 0x96, 0x96, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1943  
								0x96, 0xA5, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1944  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1945  
								0x95, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x77, //1946  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1947  
								0x96, 0xA5, 0xA6, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //1948  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x79, 0x78, 0x79, 0x77, 0x87, //1949  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x77, //1950  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1951  
								0x96, 0xA5, 0xA6, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //1952  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1953  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x78, 0x79, 0x78, 0x68, 0x78, 0x87, //1954  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1955  
								0x96, 0xA5, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //1956  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1957  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1958  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1959  
								0x96, 0xA4, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1960  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1961  
								0x96, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1962  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1963  
								0x96, 0xA4, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1964  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1965  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1966  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1967  
								0x96, 0xA4, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1968  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1969  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1970  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1971  
								0x96, 0xA4, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1972  
								0xA5, 0xB5, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1973  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1974  
								0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x77, //1975  
								0x96, 0xA4, 0xA5, 0xB5, 0xA6, 0xA6, 0x88, 0x89, 0x88, 0x78, 0x87, 0x87, //1976  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //1977  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x78, 0x87, //1978  
								0x96, 0xB4, 0x96, 0xA6, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x77, //1979  
								0x96, 0xA4, 0xA5, 0xB5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1980  
								0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x77, 0x87, //1981  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1982  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x77, //1983  
								0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x87, //1984  
								0xA5, 0xB4, 0xA6, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //1985  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1986  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x79, 0x78, 0x69, 0x78, 0x87, //1987  
								0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //1988  
								0xA5, 0xB4, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1989  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1990  
								0x95, 0xB4, 0x96, 0xA5, 0x86, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1991  
								0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //1992  
								0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1993  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1994  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x76, 0x78, 0x69, 0x78, 0x87, //1995  
								0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //1996  
								0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1997  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1998  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1999  
								0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2000  
								0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2001  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //2002  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //2003  
								0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2004  
								0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2005  
								0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2006  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //2007  
								0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x87, 0x78, 0x87, 0x86, //2008  
								0xA5, 0xB3, 0xA5, 0xB5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2009  
								0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2010  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x78, 0x87, //2011  
								0x96, 0xB4, 0xA5, 0xB5, 0xA5, 0xA6, 0x87, 0x88, 0x87, 0x78, 0x87, 0x86, //2012  
								0xA5, 0xB3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x87, //2013  
								0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2014  
								0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //2015  
								0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x87, 0x88, 0x87, 0x78, 0x87, 0x86, //2016  
								0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x87, //2017  
								0xA5, 0xB4, 0xA6, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2018  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //2019  
								0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x86, //2020  
								0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2021  
								0xA5, 0xB4, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2022  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //2023  
								0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x96, //2024  
								0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2025  
								0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2026  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //2027  
								0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x96, //2028  
								0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2029  
								0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2030  
								0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //2031  
								0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x96, //2032  
								0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x86, //2033  
								0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x78, 0x88, 0x78, 0x87, 0x87, //2034  
								0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2035  
								0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x96, //2036  
								0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2037  
								0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2038  
								0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2039  
								0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x96, //2040  
								0xA5, 0xC3, 0xA5, 0xB5, 0xA5, 0xA6, 0x87, 0x88, 0x87, 0x78, 0x87, 0x86, //2041  
								0xA5, 0xB3, 0xA5, 0xB5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2042  
								0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2043  
								0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x88, 0x87, 0x96, //2044  
								0xA5, 0xC3, 0xA5, 0xB4, 0xA5, 0xA6, 0x87, 0x88, 0x87, 0x78, 0x87, 0x86, //2045  
								0xA5, 0xB3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x87, //2046  
								0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2047  
								0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA5, 0x97, 0x87, 0x87, 0x88, 0x86, 0x96, //2048  
								0xA4, 0xC3, 0xA5, 0xA5, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x86, //2049  
								0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x78, 0x78, 0x87, 0x87}; //2050  


        private DateTime m_Date;
        public DateTime Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }

        public DateHelper()
        {
            Date = DateTime.Today;
        }
        public DateHelper(DateTime dt)
        {
            Date = dt.Date;
        }

        /// <summary>
        /// ����ָ�����ڵ�������� 
        /// </summary>
        /// <returns></returns>
        public int GetConstellation()
        {
            int Y, M, D;
            Y = m_Date.Year;
            M = m_Date.Month;
            D = m_Date.Day;
            Y = M * 100 + D;
            if (((Y >= 321) && (Y <= 419))) { return 0; }
            else if ((Y >= 420) && (Y <= 520)) { return 1; }
            else if ((Y >= 521) && (Y <= 620)) { return 2; }
            else if ((Y >= 621) && (Y <= 722)) { return 3; }
            else if ((Y >= 723) && (Y <= 822)) { return 4; }
            else if ((Y >= 823) && (Y <= 922)) { return 5; }
            else if ((Y >= 923) && (Y <= 1022)) { return 6; }
            else if ((Y >= 1023) && (Y <= 1121)) { return 7; }
            else if ((Y >= 1122) && (Y <= 1221)) { return 8; }
            else if ((Y >= 1222) || (Y <= 119)) { return 9; }
            else if ((Y >= 120) && (Y <= 218)) { return 10; }
            else if ((Y >= 219) && (Y <= 320)) { return 11; }
            else { return -1; };
        }

        /// <summary>
        /// ����ָ�����ڵ���������
        /// </summary>
        /// <returns></returns>
        public string GetConstellationName()
        {
            int Constellation;
            Constellation = GetConstellation();
            if ((Constellation >= 0) && (Constellation <= 11))
            { return ConstellationName[Constellation]; }
            else
            { return ""; };
        }

        /// <summary>
        /// ���㹫�������Ӧ�Ľ��� 0-23��-1��ʾ���ǽ���
        /// </summary>
        /// <returns></returns>
        public int l_GetLunarHolDay()
        {
            byte Flag;
            int Day, iYear, iMonth, iDay;
            iYear = m_Date.Year;
            if ((iYear < START_YEAR) || (iYear > END_YEAR))
            { return -1; };
            iMonth = m_Date.Month;
            iDay = m_Date.Day;
            Flag = gLunarHolDay[(iYear - START_YEAR) * 12 + iMonth - 1];
            if (iDay < 15)
            { Day = 15 - ((Flag >> 4) & 0x0f); }
            else
            { Day = (Flag & 0x0f) + 15; };
            if (iDay == Day)
            {
                if (iDay > 15)
                { return (iMonth - 1) * 2 + 1; }
                else
                { return (iMonth - 1) * 2; }
            }
            else
            { return -1; };
        }

        /// <summary>
        /// Formats the month.
        /// </summary>
        /// <param name="iMonth">The i month.</param>
        /// <param name="bLunar">if set to <c>true</c> [b lunar].</param>
        /// <returns></returns>
        public string FormatMonth(ushort iMonth, bool bLunar)
        {
            string szText = "�������������߰˾�ʮ";
            string strMonth;
            if ((!bLunar) && (iMonth == 1))
            { return "һ��"; }
            if (iMonth <= 10)
            {
                strMonth = "";
                strMonth = strMonth + szText.Substring(iMonth - 1, 1);
                strMonth = strMonth + "��";
                return strMonth;
            }
            if (iMonth == 11)
            { strMonth = "ʮһ"; }
            else
            { strMonth = "ʮ��"; }
            return strMonth + "��";
        }


        /// <summary>
        /// Formats the lunar day.
        /// </summary>
        /// <param name="iDay">The i day.</param>
        /// <returns></returns>
        public string FormatLunarDay(ushort iDay)
        {
            string szText1 = "��ʮإ��";
            string szText2 = "һ�����������߰˾�ʮ";
            string strDay;
            if ((iDay != 20) && (iDay != 30))
            {
                strDay = szText1.Substring((iDay - 1) / 10, 1);
                strDay = strDay + szText2.Substring((iDay - 1) % 10, 1);
            }
            else
            {
                //strDay = szText1.Substring((iDay / 10) * 2 + 1, 2); 
                strDay = szText1.Substring(iDay / 10, 1);
                strDay = strDay + "ʮ";
            }
            return strDay;
        }

        /// <summary>
        /// Gets the lunar hol day.
        /// </summary>
        /// <returns></returns>
        public string GetLunarHolDay()
        {
            ushort iYear, iMonth, iDay;
            int i;
            TimeSpan ts;
            iYear = (ushort)(m_Date.Year);
            if ((iYear < START_YEAR) || (iYear > END_YEAR))
            { return ""; };
            i = l_GetLunarHolDay();
            if ((i >= 0) && (i <= 23))
            { return LunarHolDayName[i]; }
            else
            {
                ts = m_Date - (new DateTime(START_YEAR, 1, 1));
                l_CalcLunarDate(out iYear, out iMonth, out iDay, (uint)(ts.Days));
                return FormatMonth(iMonth, true) + FormatLunarDay(iDay);
            }
        }

        /// <summary>
        /// ��������iLunarYear��������·ݣ���û�з���0 1901��1��---2050��12��
        /// </summary>
        /// <param name="iLunarYear">The i lunar year.</param>
        /// <returns></returns>
        public int GetLeapMonth(ushort iLunarYear)
        {
            byte Flag;
            if ((iLunarYear < START_YEAR) || (iLunarYear > END_YEAR))
            { return 0; };
            Flag = gLunarMonth[(iLunarYear - START_YEAR) / 2];
            if ((iLunarYear - START_YEAR) % 2 == 0)
            { return Flag >> 4; }
            else
            { return Flag & 0x0F; }
        }

        /// <summary>
        /// ��������iLunarYer������iLunarMonth�µ����������iLunarMonthΪ���£�  
        /// ����Ϊ�ڶ���iLunarMonth�µ��������������Ϊ0 1901��1��---2050��12�� 
        /// </summary>
        /// <param name="iLunarYear">The i lunar year.</param>
        /// <param name="iLunarMonth">The i lunar month.</param>
        /// <returns></returns>
        public uint LunarMonthDays(ushort iLunarYear, ushort iLunarMonth)
        {
            int Height, Low;
            int iBit;
            if ((iLunarYear < START_YEAR) || (iLunarYear > END_YEAR))
            {
                return 30;
            }
            Height = 0;
            Low = 29;
            iBit = 16 - iLunarMonth;
            if ((iLunarMonth > GetLeapMonth(iLunarYear)) && (GetLeapMonth(iLunarYear) > 0))
            {
                iBit--;
            }
            if ((gLunarMonthDay[iLunarYear - START_YEAR] & (1 << iBit)) > 0)
            {
                Low++;
            }
            if (iLunarMonth == GetLeapMonth(iLunarYear))
            {
                if ((gLunarMonthDay[iLunarYear - START_YEAR] & (1 << (iBit - 1))) > 0)
                {
                    Height = 30;
                }
                else
                {
                    Height = 29;
                }
            }
            //uint y=(uint)((uint)(Low)+(uint)(Height)); 
            return (uint)((uint)(Low) | (uint)(Height) << 16); //�ϳ�Ϊuint  
            //return y; 
        }

        /// <summary>
        /// ��������iLunarYear��������� 1901��1��---2050��12��  
        /// </summary>
        /// <param name="iLunarYear">The i lunar year.</param>
        /// <returns></returns>
        public int LunarYearDays(ushort iLunarYear)
        {
            int Days;
            uint tmp;
            if ((iLunarYear < START_YEAR) || (iLunarYear > END_YEAR))
            { return 0; };
            Days = 0;
            for (ushort i = 1; i <= 12; i++)
            {
                tmp = LunarMonthDays(iLunarYear, i);
                Days = Days + ((ushort)(tmp >> 16) & 0xFFFF); //ȡ��λ  
                Days = Days + (ushort)(tmp); //ȡ��λ  
            }
            return Days;
        }

        /// <summary>
        /// �����1901��1��1�չ�iSpanDays������������ 
        /// </summary>
        /// <param name="iYear">The i year.</param>
        /// <param name="iMonth">The i month.</param>
        /// <param name="iDay">The i day.</param>
        /// <param name="iSpanDays">The i span days.</param>
        public void l_CalcLunarDate(out ushort iYear, out ushort iMonth, out ushort iDay, uint iSpanDays)
        {
            uint tmp;
            //����1901��2��19��Ϊ����1901�����³�һ  
            //����1901��1��1�յ�2��19�չ���49��  
            if (iSpanDays < 49)
            {
                iYear = START_YEAR - 1;
                if (iSpanDays < 19)
                {
                    iMonth = 11;
                    iDay = (ushort)(11 + iSpanDays);
                }
                else
                {
                    iMonth = 12;
                    iDay = (ushort)(iSpanDays - 18);
                }
                return;
            }
            //���������1901�����³�һ����  
            iSpanDays = iSpanDays - 49;
            iYear = START_YEAR;
            iMonth = 1;
            iDay = 1;
            //������  
            tmp = (uint)LunarYearDays(iYear);
            while (iSpanDays >= tmp)
            {
                iSpanDays = iSpanDays - tmp;
                iYear++;
                tmp = (uint)LunarYearDays(iYear);
            }
            //������  
            tmp = LunarMonthDays(iYear, iMonth); //ȡ��λ  
            tmp = tmp & 0xff;
            while (iSpanDays >= tmp)
            {
                iSpanDays = iSpanDays - tmp;
                if (iMonth == GetLeapMonth(iYear))
                {
                    tmp = (LunarMonthDays(iYear, iMonth) >> 16) & 0x00FF; //ȡ��λ  
                    if (iSpanDays < tmp)
                    { break; }
                    iSpanDays = iSpanDays - tmp;
                }
                iMonth++;
                tmp = LunarMonthDays(iYear, iMonth); //ȡ��λ  
                tmp = tmp & 0xff;
            }
            //������  
            iDay = (ushort)(iDay + iSpanDays);
        }


        /// <summary>
        /// ��iYear���ʽ������ɼ��귨��ʾ���ַ���
        /// </summary>
        /// <returns></returns>
        public string FormatLunarYear()
        {
            string strYear;
            string szText1 = "���ұ����켺�����ɹ�";
            string szText2 = "�ӳ���î������δ�����纥";
            string szText3 = "��ţ������������Ｆ����";
            ushort iYear;
            iYear = (ushort)(m_Date.Year);
            strYear = szText1.Substring((iYear - 4) % 10, 1);
            strYear = strYear + szText2.Substring((iYear - 4) % 12, 1);
            strYear = strYear + " ";
            strYear = strYear + szText3.Substring((iYear - 4) % 12, 1);
            strYear = strYear + "��";
            return strYear;
        }

        /// <summary>
        /// Smarts the date display.
        /// </summary>
        /// <param name="strDatetime">The STR datetime.</param>
        /// <returns></returns>
        public static string SmartDateDisplay(string strDatetime)
        {
            if (string.IsNullOrEmpty(strDatetime) == true)
            {
                return string.Empty;
            }
            DateTime now = DateTime.Now;

            strDatetime = strDatetime.Replace(now.ToShortDateString(), "Today");
            strDatetime = strDatetime.Replace(now.AddDays(-1).ToShortDateString(), "Yesterday");

            return strDatetime;
        }
    } //class CNDate  
    #endregion
}