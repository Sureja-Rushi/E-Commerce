import React from "react";
import "./ProductCard.css";
import { useNavigate } from "react-router";

const ProductCard = ({ product }) => {

  const navigate = useNavigate();

  return (
    <div onClick={() => navigate(`/product/${4}`)} className="productCard w-[15rem] m-3 transation-all cursor-pointer">
      <div className="h-[20rem]">
        <img
          className="h-full w-full object-cover object-left-top"
          src={product.imageUrl}
          alt=""
        />
      </div>
      <div className="textPart bg-white p-3">
        <div>
          <p>{product.title}</p>
          <p className="font-bold opacity-60">{product.brand}</p>
        </div>
        <div className="flex items-center space-x-2">
          <p className="font-semibold">Rs. {product.discountedPrice}</p>
          <p className="line-through opacity-70">Rs. {product.price}</p>
          <p className="text-green-600 font-semibold">{product.discountPersent}% off</p>
        </div>
      </div>
    </div>
  );
};

export default ProductCard;
