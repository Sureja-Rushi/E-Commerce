import { Button, IconButton } from "@mui/material";
import React from "react";
import RemoveCircleOutlineOutlinedIcon from "@mui/icons-material/RemoveCircleOutlineOutlined";
import AddCircleOutlineOutlinedIcon from "@mui/icons-material/AddCircleOutlineOutlined";
import DeleteOutlineOutlinedIcon from "@mui/icons-material/DeleteOutlineOutlined";
import { useDispatch } from "react-redux";
import { addItemToCart, removeCartItem } from "../../../State/Cart/Action";

const CartItem = ({item, disable}) => {

    const dispatch = useDispatch();

    const handleRemoveSingleItem = () => {
        const data = {productId: item.product.id, sizeName: item.size}
        dispatch(removeCartItem(data, false));
    }

    const handleAddItem = () => {
        const data = {productId: item.product.id, sizeName: item.size}
        dispatch(addItemToCart(data));
    }

    const handleRemoveAllItem = () => {
        const data = {productId: item.product.id, sizeName: item.size}
        dispatch(removeCartItem(data, true));
    }

    console.log("item: ", item);

  return (
    <div className="p-5 shadow-lg border rounded-md">
      <div className="flex item-center">
        <div className="w-[5rem] h-[5rem] lg:w-[9rem] lg:h-[9rem]">
          <img
            className="w-full h-full object-cover object-top"
            src={item.product.imageUrl}
            alt=""
          />
        </div>

        <div className="ml-5 space-y-1">
          <p className="font-semibold">{item.product.title}</p>
          <p className="opacity-70 mt-2 font-semibold">{item.product.brand}</p>
          <p className="opacity-70">Size: {item.size}, {item.product.color}</p>
          <div className="flex space-x-3 items-center text-gray-900 pt-6">
            <p className="font-semibold">Rs. {item.discountedPrice}</p>
            <p className="opacity-50 line-through">Rs. {item.price}</p>
            <p className="text-green-600 font-semibold">{item.product.discountPercent}% Off</p>
          </div>
        </div>
      </div>
      <div className="lg:flex items-center lg:space-x-10 pt-4">
        <div className="flex items-center space-x-2">
          <IconButton sx={{ color: "red" }} onClick={handleRemoveSingleItem} disabled={item.quantity <= 1}>
            <RemoveCircleOutlineOutlinedIcon />
          </IconButton>
          <span className="py-1 px-7 border rounded-md">{item.quantity}</span>
          <IconButton sx={{ color: "green" }} onClick={handleAddItem}>
            <AddCircleOutlineOutlinedIcon />
          </IconButton>
        </div>
        <div>
          <Button sx={{ color: "red" }} className="font-semibold" onClick={handleRemoveAllItem}>
            {/* <DeleteOutlineOutlinedIcon /> */}
            Remove
          </Button>
        </div>
      </div>
    </div>
  );
};

export default CartItem;
