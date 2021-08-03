const tl = gsap.timeline({ defaults: { ease: "power1.out" } });

tl.to('.hide_text_', { y: "0%", duration: 1, });
tl.to(".slider_", { y: "-100%", duration: 1.5, delay: 0.5 });
tl.to(".intro_", { y: "-100%", duration: 1 }, "-=1");
tl.fromTo(".navbarr", { opacity: 0 }, { opacity: 1, duration: 0.5  });
tl.fromTo(".home-content", { opacity: 0 }, { opacity: 1, duration: 0.5  });