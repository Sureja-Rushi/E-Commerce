import React, { useEffect } from "react";
import AddressCard from "../addressCard/AddressCard";
import OrderTracker from "./OrderTracker";
import { Box, Grid } from "@mui/material";
import StarBorderRoundedIcon from "@mui/icons-material/StarBorderRounded";
import { deepPurple } from "@mui/material/colors";
import { useParams } from "react-router";
import { useDispatch, useSelector } from "react-redux";
import { getOrderById } from "../../../State/Order/Action";

const OrderDetails = () => {
  const params = useParams();
  const dispatch = useDispatch();
  const { order } = useSelector((store) => store);

  const totalQuantity = order.order?.orderItems.reduce(
    (sum, item) => sum + item.quantity,
    0
  );

  const statusMap = {
    placed: 1,
    confirmed: 2,
    shipped: 3,
    out_of_delivery: 4,
    delivered: 5,
  };

  useEffect(() => {
    dispatch(getOrderById(params.orderId));
    console.log("order details: ", order.order);
  }, [params.orderId]);

  console.log("status: ", order.order?.orderStatus.toLowerCase())

  return (
    <div className="px-5 lg:px-16">
      <div className="p-5 shadow-lg rounded-md border">
        <h1 className="font-bold text-xl py-5">Delivery Address</h1>
        <AddressCard address={order.order?.shippingAddress} />
      </div>
      <div className="py-14">
        <OrderTracker activeStep={statusMap[order.order?.orderStatus.toLowerCase()]} />
      </div>

      <div className="p-5 border flex justify-between mb-4 rounded-md shadow-lg shadow-gray-300">
        <div className="space-y-2">
          <p className="opacity-80 text-md font-semibold">
            Total Items: {totalQuantity}
          </p>
          <p className="opacity-80 text-md font-semibold">
            Order Date: {order.order?.orderDate.split("T")[0]}
          </p>
          <p className="text-gray-700 mr-2 text-md font-bold">
            {order.order?.orderStatus}
          </p>
        </div>
        <div>
          <div className="flex gap-4">
            <p className="font-semibold line-through text-gray-700">
              Rs. {order.order?.totalPrice}
            </p>
            <p className="font-semibold text-green-700">
              Rs. {order.order?.totalDiscountedPrice}
            </p>
          </div>
          <div>
            <p className="font-semibold text-red-700">
              Got Discount of Rs. {order.order?.discount}
            </p>
          </div>
        </div>
      </div>

      <Grid className="space-y-5" container>
        {order.order?.orderItems.map((item) => (
          <Grid
            item
            container
            className="shadow-xl shadow-gray-300 rounded-md p-5 border"
            sx={{ alignItems: "center", justifyContent: "space-between" }}
          >
            <Grid item xs={6}>
              <div className="flex items-center space-x-4">
                <img
                  className="h-[7rem] w-[7rem] object-cover object-top rounded-sm"
                  src={item.product.imageUrl}
                  alt=""
                />
                <div className="ml-5 space-y-2">
                  <p className="font-semibold">
                    {item.product.title}{" "}
                    <span className="opacity-75 text-gray-800 font-bold border border-gray-600 border-opacity-50 rounded-sm px-2 py-1">
                      {" "}
                      X {item.quantity}
                    </span>{" "}
                  </p>
                  <p className="space-x-5 text-sm">
                    <span>Color: {item.product.color}</span>{" "}
                    <span>Size: {item.size}</span>
                  </p>
                  <p className="opacity-70">{item.product.brand}</p>
                  <div className="flex  gap-4">
                    <p className="font-semibold line-through text-gray-700">
                      Rs. {item.price}
                    </p>
                    <p className="font-semibold text-green-700">
                      Rs. {item.discountedPrice}
                    </p>
                  </div>
                </div>
              </div>
            </Grid>

            <Grid item>
              <Box sx={{ color: deepPurple[500] }}>
                <StarBorderRoundedIcon
                  sx={{ fontSize: "2.5rem" }}
                  className="px-2 font-semibold"
                />
                <span className="font-semibold">Rate & Review Product</span>
              </Box>
            </Grid>
          </Grid>
        ))}
      </Grid>
    </div>
  );
};

export default OrderDetails;
