import React from "react";
import AddressCard from "../addressCard/AddressCard";
import OrderTracker from "./OrderTracker";
import { Box, Grid } from "@mui/material";
import StarBorderRoundedIcon from "@mui/icons-material/StarBorderRounded";
import { deepPurple } from "@mui/material/colors";

const OrderDetails = () => {
  return (
    <div className="px-5 lg:px-16">
      <div>
        <h1 className="font-bold text-xl py-5">Delivery Address</h1>
        <AddressCard />
      </div>
      <div className="py-14">
        <OrderTracker activeStep={3} />
      </div>

      <Grid className="space-y-5" container>
        {[1,1,1,1,1].map((item) => <Grid
          item
          container
          className="shadow-xl shadow-gray-400 rounded-md p-5 border"
          sx={{ alignItems: "center", justifyContent: "space-between" }}
        >
          <Grid item xs={6}>
            <div className="flex items-center space-x-4">
              <img
                className="h-[7rem] w-[7rem] object-cover object-top rounded-sm"
                src="https://rukminim1.flixcart.com/image/612/612/xif0q/jean/2/q/g/30-jeans-kneecut-black-crishtaliyo-2fashion-original-imagqy6gzmpwqkge.jpeg?q=70"
                alt=""
              />
              <div className="ml-5 space-y-2">
                <p className="font-semibold">Men Slim Mid Rise Black Jeans</p>
                <p className="space-x-5 text-sm">
                  <span>Color: Black</span> <span>Size: M</span>
                </p>
                <p className="opacity-70">Peter England</p>
                <p className="font-semibold">Rs. 1099</p>
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
        </Grid> )}
      </Grid>
    </div>
  );
};

export default OrderDetails;
