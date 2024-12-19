import { Grid } from "@mui/material";
import React from "react";
import LocalShippingOutlinedIcon from "@mui/icons-material/LocalShippingOutlined";
import { useNavigate } from 'react-router-dom'

const OrderCard = () => {

  const navigate = useNavigate();

  return (
    <div onClick={() => navigate(`/account/order/${4}`)} className="p-5 shadow-md shadow-gray-300 hover:shadow-2xl border">
      <Grid container sx={{ justifyContent: "space-between" }} spacing={2}>
        <Grid item xs={6}>
          <div className="flex cursor-pointer">
            <img
              className="h-[5rem] w-[5rem] object-cover object-top"
              src="https://rukminim1.flixcart.com/image/612/612/xif0q/jean/2/q/g/30-jeans-kneecut-black-crishtaliyo-2fashion-original-imagqy6gzmpwqkge.jpeg?q=70"
              alt=""
            />

            <div className="ml-5 space-y-2">
              <p className="">Men Slim Mid Rise Black Jeans</p>
              <p className="opacity-70 text-sm font-semibold">Size: M</p>
              <p className="opacity-70 text-sm font-semibold">Color: Black</p>
            </div>
          </div>
        </Grid>

        <Grid item xs={2}>
          <p>Rs. 1099</p>
        </Grid>

        <Grid item xs={4}>
          {true && (
            <p>
              <LocalShippingOutlinedIcon
                sx={{fontSize: "1.5rem"}}
                className="text-gray-600 mr-2 text-md"
              />
              <span>Delivered On 15 December 2024</span>
            </p>
          )}

          {false && (
            <p>
              <LocalShippingOutlinedIcon
                sx={{ fontSize: "1.5rem" }}
                className="text-green-600 mr-2 text-md"
              />
              <span>Expected Delivery On 25 December 2024</span>
            </p>
          )}
        </Grid>
      </Grid>
    </div>
  );
};

export default OrderCard;
