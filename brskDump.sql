PGDMP     "                    |            ShopBD    15.1    15.1                 0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            !           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            "           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            #           1262    33141    ShopBD    DATABASE     |   CREATE DATABASE "ShopBD" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
    DROP DATABASE "ShopBD";
                postgres    false            �            1259    33156    Cart    TABLE     �   CREATE TABLE public."Cart" (
    "CartID " integer NOT NULL,
    "UserID " integer NOT NULL,
    "ProductID " integer NOT NULL,
    "Quantity " integer
);
    DROP TABLE public."Cart";
       public         heap    postgres    false            �            1259    33168 
   OrderItems    TABLE     �   CREATE TABLE public."OrderItems" (
    "OrderItemID" integer NOT NULL,
    "OrderID " integer NOT NULL,
    "ProductID " integer NOT NULL,
    "Quantity " integer
);
     DROP TABLE public."OrderItems";
       public         heap    postgres    false            �            1259    33161    Orders    TABLE     �   CREATE TABLE public."Orders" (
    "OrderID " integer NOT NULL,
    "UserID " integer NOT NULL,
    "Status " text NOT NULL,
    "Date" timestamp without time zone NOT NULL
);
    DROP TABLE public."Orders";
       public         heap    postgres    false            �            1259    33173    Products    TABLE     �   CREATE TABLE public."Products" (
    "ProductID" integer NOT NULL,
    "ProductName" text NOT NULL,
    "Description" text NOT NULL,
    "Price" numeric NOT NULL,
    "StockQuantity" integer
);
    DROP TABLE public."Products";
       public         heap    postgres    false            �            1259    33149    Role    TABLE     \   CREATE TABLE public."Role" (
    "RoleID" integer NOT NULL,
    "RoleName" text NOT NULL
);
    DROP TABLE public."Role";
       public         heap    postgres    false            �            1259    33142    User    TABLE     �   CREATE TABLE public."User" (
    "UserID" integer NOT NULL,
    "Username " text NOT NULL,
    "PasswordHash" text NOT NULL,
    "RoleID" integer NOT NULL,
    "Login" text
);
    DROP TABLE public."User";
       public         heap    postgres    false                      0    33156    Cart 
   TABLE DATA           Q   COPY public."Cart" ("CartID ", "UserID ", "ProductID ", "Quantity ") FROM stdin;
    public          postgres    false    216   P                  0    33168 
   OrderItems 
   TABLE DATA           \   COPY public."OrderItems" ("OrderItemID", "OrderID ", "ProductID ", "Quantity ") FROM stdin;
    public          postgres    false    218   m                  0    33161    Orders 
   TABLE DATA           L   COPY public."Orders" ("OrderID ", "UserID ", "Status ", "Date") FROM stdin;
    public          postgres    false    217   �                  0    33173    Products 
   TABLE DATA           i   COPY public."Products" ("ProductID", "ProductName", "Description", "Price", "StockQuantity") FROM stdin;
    public          postgres    false    219   /!                 0    33149    Role 
   TABLE DATA           6   COPY public."Role" ("RoleID", "RoleName") FROM stdin;
    public          postgres    false    215   �!                 0    33142    User 
   TABLE DATA           Z   COPY public."User" ("UserID", "Username ", "PasswordHash", "RoleID", "Login") FROM stdin;
    public          postgres    false    214   �!       }           2606    33160    Cart Cart_pkey 
   CONSTRAINT     W   ALTER TABLE ONLY public."Cart"
    ADD CONSTRAINT "Cart_pkey" PRIMARY KEY ("CartID ");
 <   ALTER TABLE ONLY public."Cart" DROP CONSTRAINT "Cart_pkey";
       public            postgres    false    216            �           2606    33172    OrderItems OrderItems_pkey 
   CONSTRAINT     g   ALTER TABLE ONLY public."OrderItems"
    ADD CONSTRAINT "OrderItems_pkey" PRIMARY KEY ("OrderItemID");
 H   ALTER TABLE ONLY public."OrderItems" DROP CONSTRAINT "OrderItems_pkey";
       public            postgres    false    218                       2606    33167    Orders Orders_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public."Orders"
    ADD CONSTRAINT "Orders_pkey" PRIMARY KEY ("OrderID ");
 @   ALTER TABLE ONLY public."Orders" DROP CONSTRAINT "Orders_pkey";
       public            postgres    false    217            �           2606    33179    Products Products_pkey 
   CONSTRAINT     a   ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "Products_pkey" PRIMARY KEY ("ProductID");
 D   ALTER TABLE ONLY public."Products" DROP CONSTRAINT "Products_pkey";
       public            postgres    false    219            {           2606    33155    Role Role_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public."Role"
    ADD CONSTRAINT "Role_pkey" PRIMARY KEY ("RoleID");
 <   ALTER TABLE ONLY public."Role" DROP CONSTRAINT "Role_pkey";
       public            postgres    false    215            y           2606    33148    User User_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("UserID");
 <   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_pkey";
       public            postgres    false    214            �           2606    33190    Cart Fk_Cart_Product    FK CONSTRAINT     �   ALTER TABLE ONLY public."Cart"
    ADD CONSTRAINT "Fk_Cart_Product" FOREIGN KEY ("ProductID ") REFERENCES public."Products"("ProductID") NOT VALID;
 B   ALTER TABLE ONLY public."Cart" DROP CONSTRAINT "Fk_Cart_Product";
       public          postgres    false    219    216    3203            �           2606    33185    Cart Fk_Cart_User    FK CONSTRAINT     �   ALTER TABLE ONLY public."Cart"
    ADD CONSTRAINT "Fk_Cart_User" FOREIGN KEY ("UserID ") REFERENCES public."User"("UserID") NOT VALID;
 ?   ALTER TABLE ONLY public."Cart" DROP CONSTRAINT "Fk_Cart_User";
       public          postgres    false    3193    214    216            �           2606    33200    OrderItems Fk_ItemOrder_Orders    FK CONSTRAINT     �   ALTER TABLE ONLY public."OrderItems"
    ADD CONSTRAINT "Fk_ItemOrder_Orders" FOREIGN KEY ("OrderID ") REFERENCES public."Orders"("OrderID ") NOT VALID;
 L   ALTER TABLE ONLY public."OrderItems" DROP CONSTRAINT "Fk_ItemOrder_Orders";
       public          postgres    false    3199    217    218            �           2606    33195    Orders Fk_Orders_User    FK CONSTRAINT     �   ALTER TABLE ONLY public."Orders"
    ADD CONSTRAINT "Fk_Orders_User" FOREIGN KEY ("UserID ") REFERENCES public."User"("UserID") NOT VALID;
 C   ALTER TABLE ONLY public."Orders" DROP CONSTRAINT "Fk_Orders_User";
       public          postgres    false    214    217    3193            �           2606    33180    User Fk_User_Role    FK CONSTRAINT     �   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "Fk_User_Role" FOREIGN KEY ("RoleID") REFERENCES public."Role"("RoleID") NOT VALID;
 ?   ALTER TABLE ONLY public."User" DROP CONSTRAINT "Fk_User_Role";
       public          postgres    false    214    215    3195            �           2606    33205    OrderItems Fk_itemOrder_Product    FK CONSTRAINT     �   ALTER TABLE ONLY public."OrderItems"
    ADD CONSTRAINT "Fk_itemOrder_Product" FOREIGN KEY ("ProductID ") REFERENCES public."Products"("ProductID") NOT VALID;
 M   ALTER TABLE ONLY public."OrderItems" DROP CONSTRAINT "Fk_itemOrder_Product";
       public          postgres    false    219    3203    218                  x������ � �         4   x��� 0�7��9�I�u����8]��/�[^Vy#�پ��$?�%         n   x�3�4�0I�����]l���b���[9u3��2�2K*��q�0202�5 "#C+C+=cKc.ⴘX��r�t�!H�d�dӅ}6!I��qqq ��SW            x�m�1
�P��S� "��9��'�0��b��x�	�fo�6v�o�M#�)|X(~���pc�K�gfFޞ����ߙI���<0ŘYئ�XX�5If����E��!YS���!��V�[U�~3UN         ,   x�3�tL���S�,.I͍��2�-N-2�a2�\1z\\\ 4��         e   x�3�0���[/��$wr��r�H.#�S.칰�b�ņ;�R��)�F �˘��:��Ϊ�dNc�e�HKπP@#�4�)�...�Q�PW� @�2:     