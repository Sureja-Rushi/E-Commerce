import { Button, Card, CardContent, styled, Typography } from "@mui/material";
import React from "react";
import trophy from "../../../public/Images/trophy-removebg-preview.png";

const TriangleImg = styled("img")({
  right: 0,
  bottom: 0,
  height: 170,
  position: "absolute",
});

const TrophyImg = styled("img")({
  right: 36,
  bottom: 20,
  height: 98,
  position: "absolute",
});

const Achievement = () => {
  return (
    <div>
      <Card className="" sx={{ position: "relative", bgcolor: "#242B2F", color:"white" }}>
        <CardContent>
          <Typography variant="h6" sx={{ letterSpacing: "0.25px" }}>
            Trendy Fashions
          </Typography>
          <Typography variant="body2">Congratulations 🎊</Typography>
          <Typography variant="h5" sx={{my:3.1}}>420.8k</Typography>
          <Button size="small" variant="contained">
            View Sales
          </Button>
          <TriangleImg src="" />
          <TrophyImg src={trophy} />
        </CardContent>
      </Card>
    </div>
  );
};

export default Achievement;
