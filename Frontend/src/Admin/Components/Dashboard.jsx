import { Grid } from "@mui/material";
import React from "react";
import Achievement from "./Achievement";
import MonthlyOverview from "./MonthlyOverview";
import OrdersTableView from "../view/OrderTableView";

const Dashboard = () => {
  return (
    <div className="p-10">
      <Grid container spacing={2}>
        <Grid item xs={12} md={4}>
          <Achievement />
        </Grid>
        <Grid item xs={12} md={8}>
          <MonthlyOverview />
        </Grid>
        <Grid item xs={12} md={6}>
          <OrdersTableView />
        </Grid>
      </Grid>
    </div>
  );
};

export default Dashboard;
