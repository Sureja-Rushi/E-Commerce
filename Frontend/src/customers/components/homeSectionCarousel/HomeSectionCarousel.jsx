// import React, { useState } from "react";
// import AliceCarousel from "react-alice-carousel";
// import HomeSectionCard from "../homeSectionCard/HomeSectionCard";
// import KeyboardDoubleArrowLeftIcon from "@mui/icons-material/KeyboardDoubleArrowLeft";
// import { Button } from "@mui/material";
// import { men_kurta } from "../../../Data/men_kurta";

// const HomeSectionCarousel = () => {
//   const [activeIndex, setActiveIndex] = useState(0);
//   const responsive = {
//     0: { items: 1 },
//     720: { items: 3 },
//     1024: { items: 4 },
//   };

//   // const slidePrev=()=>setActiveIndex(activeIndex-1);
//   // const slideNext=()=>setActiveIndex(activeIndex+1);

//   const slidePrev = () => {
//     if (activeIndex > 0) setActiveIndex(activeIndex - 1);
//   };

//   const slideNext = () => {
//     if (activeIndex < items.length - 1) setActiveIndex(activeIndex + 1);
//   };

//   // const syncActiveIndex=({item})=>setActiveIndex(item)

//   const syncActiveIndex = ({ item }) => setActiveIndex(item);

//   const items = men_kurta
//     .slice(0, 10)
//     .map((item) => <HomeSectionCard product={item} />);

//   const visibleItems = responsive[1024]?.items || 4;
//   const isAtStart = activeIndex === 0;
//   const isAtEnd = activeIndex >= items.length - visibleItems;

//   return (
//     <div className="border">
//       <div className="relative p-5">
//         <AliceCarousel
//           items={items}
//           disableButtonsControls
//           responsive={responsive}
//           disableDotsControls
//           onSlideChanged={syncActiveIndex}
//           activeIndex={activeIndex}
//         />
//         {!isAtEnd && (
//           <Button
//             onClick={slideNext}
//             variant="contained"
//             className="z-50 bg-white"
//             sx={{
//               position: "absolute",
//               top: "8rem",
//               right: "0rem",
//               transform: "translateX(50%) rotate(90deg)",
//               bgcolor: "white",
//             }}
//             aria-label="next"
//           >
//             <KeyboardDoubleArrowLeftIcon
//               sx={{ transform: "rotate(90deg)", color: "black" }}
//             />
//           </Button>
//         )}

//         {!isAtStart && (
//           <Button
//             onClick={slidePrev}
//             variant="contained"
//             className="z-50 bg-white"
//             sx={{
//               position: "absolute",
//               top: "8rem",
//               left: "0rem",
//               transform: "translateX(-50%) rotate(-90deg)",
//               bgcolor: "white",
//             }}
//             aria-label="previous"
//           >
//             <KeyboardDoubleArrowLeftIcon
//               sx={{ transform: "rotate(90deg)", color: "black" }}
//             />
//           </Button>
//         )}
//       </div>
//     </div>
//   );
// };

// export default HomeSectionCarousel;



import React, { useRef, useState, useEffect } from "react";
import AliceCarousel from "react-alice-carousel";
import HomeSectionCard from "../homeSectionCard/HomeSectionCard";
import KeyboardDoubleArrowLeftIcon from "@mui/icons-material/KeyboardDoubleArrowLeft";
import { Button } from "@mui/material";

const HomeSectionCarousel = ({data, sectionName}) => {
  const carouselRef = useRef(null); // Ref for AliceCarousel
  const [currentIndex, setCurrentIndex] = useState(0); // Track active slide
  const [visibleItems, setVisibleItems] = useState(4); // Track visible items based on screen size

  const responsive = {
    0: { items: 1 },
    720: { items: 3 },
    1024: { items: 4 },
  };

  const items = data.slice(0, 10).map((item) => (
    <HomeSectionCard product={item} />
  ));

  // Dynamically calculate visible items on resize
  useEffect(() => {
    const updateVisibleItems = () => {
      if (window.innerWidth >= 1024) setVisibleItems(4);
      else if (window.innerWidth >= 720) setVisibleItems(3);
      else setVisibleItems(1);
    };

    updateVisibleItems();
    window.addEventListener("resize", updateVisibleItems);
    return () => window.removeEventListener("resize", updateVisibleItems);
  }, []);

  // Slide Handlers
  const slidePrev = () => {
    if (currentIndex > 0) {
      carouselRef.current?.slidePrev();
    }
  };

  const slideNext = () => {
    if (currentIndex < items.length - visibleItems) {
      carouselRef.current?.slideNext();
    }
  };

  // Update current index when the slide changes
  const onSlideChanged = ({ item }) => {
    setCurrentIndex(item);
  };

  return (
    <div className="border">
      <h2 className="text-2xl font-extrabold text-gray-800 py-3 px-8">{sectionName}</h2>
      <div className="relative px-5 py-3">
        <AliceCarousel
          ref={carouselRef} // Attach the ref to AliceCarousel
          items={items}
          disableButtonsControls
          responsive={responsive}
          disableDotsControls
          onSlideChanged={onSlideChanged}
          activeIndex={currentIndex}
        />

        {/* Previous Button: Hidden at Start */}
        {currentIndex > 0 && (
          <Button
            onClick={slidePrev}
            variant="contained"
            className="z-50 bg-white"
            sx={{
              position: "absolute",
              top: "8rem",
              left: "0rem",
              transform: "translateX(-50%) rotate(-90deg)",
              bgcolor: "white",
            }}
            aria-label="previous"
          >
            <KeyboardDoubleArrowLeftIcon
              sx={{ transform: "rotate(90deg)", color: "black" }}
            />
          </Button>
        )}

        {/* Next Button: Hidden at End */}
        {currentIndex < items.length - visibleItems && (
          <Button
            onClick={slideNext}
            variant="contained"
            className="z-50 bg-white"
            sx={{
              position: "absolute",
              top: "8rem",
              right: "0rem",
              transform: "translateX(50%) rotate(90deg)",
              bgcolor: "white",
            }}
            aria-label="next"
          >
            <KeyboardDoubleArrowLeftIcon
              sx={{ transform: "rotate(90deg)", color: "black" }}
            />
          </Button>
        )}
      </div>
    </div>
  );
};

export default HomeSectionCarousel;
