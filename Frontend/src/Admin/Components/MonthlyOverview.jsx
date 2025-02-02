import React from "react";
import { TrendingUp } from "@mui/icons-material";
import AccountCircleRoundedIcon from "@mui/icons-material/AccountCircleRounded";
import DevicesOutlinedIcon from "@mui/icons-material/DevicesOutlined";
import AttachMoneyOutlinedIcon from "@mui/icons-material/AttachMoneyOutlined";
import MoreVertOutlinedIcon from "@mui/icons-material/MoreVertOutlined";
import {
  Avatar,
  Box,
  Card,
  CardContent,
  CardHeader,
  Grid,
  IconButton,
  Typography,
} from "@mui/material";

const salesData = [
  {
    stats: "245K",
    title: "Sales",
    color: "#E8BD0D",
    icon: <TrendingUp sx={{ fontSize: "1.75rem" }} />,
  },
  {
    stats: "12.5K",
    title: "Customers",
    color: "#22CB5C",
    icon: <AccountCircleRoundedIcon sx={{ fontSize: "1.75rem" }} />,
  },
  {
    stats: "1.54K",
    title: "Products",
    color: "#DE4839",
    icon: <DevicesOutlinedIcon sx={{ fontSize: "1.75rem" }} />,
  },
  {
    stats: "88K",
    title: "Revenue",
    color: "#12B0E8",
    icon: <AttachMoneyOutlinedIcon sx={{ fontSize: "1.75rem" }} />,
  },
];

const renderStats = () => {
  return salesData.map((item, index) => (
    <Grid iem xs={12} sm={3} key={index}>
      <Box sx={{ display: "flex", alignItems: "center" }}>
        <Avatar
          variant="rounded"
          sx={{
            mr: 3,
            width: 44,
            height: 44,
            boxShadow: 3,
            color: "white",
            backgroundColor: `${item.color}`,
          }}
        >
          {item.icon}
        </Avatar>
        <Box sx={{ display: "flex", flexDirection: "column" }}>
          <Typography variant="caption">{item.title}</Typography>
          <Typography variant="h6">{item.stats}</Typography>
        </Box>
      </Box>
    </Grid>
  ));
};

const MonthlyOverview = () => {
  return (
    <Card sx={{ bgcolor: "#242B2F", color:"white" }} elevation={0}>
      <CardHeader
        title="Monthly Overview"
        action={
          <IconButton size="small">
            <MoreVertOutlinedIcon sx={{color:"white"}} />
          </IconButton>
        }
        subheader={
          <Typography variant="body2">
            <Box
              component="span"
              sx={{ fontWeight: 600, mr:1 }}
            >
              Total 48.3% growth
            </Box>
            this month
          </Typography>
        }
        titleTypographyProps={{
          sx: {
            mb: 2.5,
            lineHeight: "2rem !imporant",
            letterSpacing: "0.15px !important",
          },
        }}
      />

        <CardContent sx={{pt:theme => `${theme.spacing(3)} !important`}}>
            <Grid container spacing={[5,0]}>
                {renderStats()}
            </Grid>
        </CardContent>

    </Card>
  );
};

export default MonthlyOverview;
