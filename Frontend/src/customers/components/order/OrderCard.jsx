import { Grid } from "@mui/material";
import React from "react";
import LocalShippingOutlinedIcon from "@mui/icons-material/LocalShippingOutlined";
import { useNavigate } from "react-router-dom";

const OrderCard = ({ order }) => {
  const navigate = useNavigate();
  console.log("order items: ", order.orderItems);
  const totalQuantity = order.orderItems.reduce(
    (sum, item) => sum + item.quantity,
    0
  );

  return (
    <div
      onClick={() => navigate(`/account/order/${order.id}`)}
      className="p-5 shadow-md shadow-gray-300 hover:shadow-2xl border"
    >
      <Grid container sx={{ justifyContent: "space-between" }} spacing={2}>
        <Grid item xs={6}>
          <div className="flex cursor-pointer">
            {/* <img
              className="h-[5rem] w-[5rem] object-cover object-top"
              src="https://rukminim1.flixcart.com/image/612/612/xif0q/jean/2/q/g/30-jeans-kneecut-black-crishtaliyo-2fashion-original-imagqy6gzmpwqkge.jpeg?q=70"
              alt=""
            /> */}

            <div className="ml-5 space-y-2">
              <p className="">
                {order.shippingAddress.firstName +
                  " " +
                  order.shippingAddress.lastName}
              </p>
              <p className="opacity-70 text-sm font-semibold">
                Total Items: {totalQuantity}
              </p>
              <p className="opacity-70 text-sm font-semibold">
                Order Date: {order.orderDate.split("T")[0]}
              </p>
            </div>
          </div>
        </Grid>

        <Grid container item xs={6}>
          <Grid item xs={6}>
            <p>Rs. {order.totalDiscountedPrice}</p>
          </Grid>
          <Grid item xs={6}>
            {order.orderStatus != "DELIVERED" && (
              <p>
                {/* <LocalShippingOutlinedIcon
                sx={{fontSize: "1.5rem"}}
                className="text-gray-600 mr-2 text-md"
              /> */}
                <span className="text-gray-600 mr-2 text-md font-bold">
                  {order.orderStatus}
                </span>
              </p>
            )}

            {order.orderStatus == "DELIVERED" && (
              <p>
                {/* <LocalShippingOutlinedIcon
                sx={{ fontSize: "1.5rem" }}
                className="text-green-600 mr-2 text-md"
              /> */}
                <span className="text-green-600 mr-2 text-md font-bold">
                  {order.orderStatus}
                </span>
              </p>
            )}
          </Grid>
          <Grid item={12}>
            <p className="text-lg text-gray-600 font-semibold">{order.shippingAddress.street + " " + order.shippingAddress.city + " " + order.shippingAddress.state + " - " + order.shippingAddress.zipCode}</p>
          </Grid>
        </Grid>
      </Grid>
    </div>
  );
};

export default OrderCard;
