import re

with open("components/qr/qr-form.tsx", "r") as f:
    content = f.read()

# 1. Update onSubmit to add onError
onSubmit_old = """  const onSubmit = async (data: FormData) => {
    setIsSubmitting(true);
    try {
      const tagsArray = data.tags ? data.tags.split(",").map(t => t.trim()).filter(Boolean) : [];
      
      if (isEditing && initialData) {
        const updateDto: UpdateQRCodeDto = {
          ...data,
          tags: tagsArray,
          status: initialData.status,
          expirationDate: data.expirationDate ? new Date(data.expirationDate).toISOString() : undefined,
          password: data.passwordProtected ? data.password : "",
        };
        await qrService.updateQRCode(initialData.id, updateDto);
        toast.success("QR Code updated successfully");
      } else {
        const createDto: CreateQRCodeDto = {
          ...data,
          tags: tagsArray,
          expirationDate: data.expirationDate ? new Date(data.expirationDate).toISOString() : undefined,
        };
        await qrService.createQRCode(createDto);
        toast.success("QR Code created successfully");
      }
      router.push("/dashboard/qr");
      router.refresh();
    } catch (error: unknown) {
      toast.error((error as { response?: { data?: { Message?: string } } }).response?.data?.Message || "An error occurred");
    } finally {
      setIsSubmitting(false);
    }
  };"""

onSubmit_new = """  const onSubmit = async (data: FormData) => {
    setIsSubmitting(true);
    try {
      const tagsArray = data.tags ? data.tags.split(",").map(t => t.trim()).filter(Boolean) : [];
      
      if (isEditing && initialData) {
        const updateDto: UpdateQRCodeDto = {
          ...data,
          tags: tagsArray,
          status: initialData.status,
          expirationDate: data.expirationDate ? new Date(data.expirationDate).toISOString() : undefined,
          password: data.passwordProtected ? data.password : "",
        };
        await qrService.updateQRCode(initialData.id, updateDto);
        toast.success("QR Code updated successfully");
      } else {
        const createDto: CreateQRCodeDto = {
          ...data,
          tags: tagsArray,
          expirationDate: data.expirationDate ? new Date(data.expirationDate).toISOString() : undefined,
        };
        await qrService.createQRCode(createDto);
        toast.success("QR Code created successfully");
      }
      router.push("/dashboard/qr");
      router.refresh();
    } catch (error: unknown) {
      console.error("Submission error:", error);
      toast.error((error as { response?: { data?: { Message?: string } } }).response?.data?.Message || "An error occurred during submission");
    } finally {
      setIsSubmitting(false);
    }
  };

  const onError = (errors: any) => {
    console.error("Form validation errors:", errors);
    toast.error("Please fill in all required fields correctly.");
  };"""

content = content.replace(onSubmit_old, onSubmit_new)

# 2. Update form onSubmit
content = content.replace('<form onSubmit={handleSubmit(onSubmit)}', '<form onSubmit={handleSubmit(onSubmit, onError)}')

# 3. Move "Visuals & Layout Settings" into the accordion
# Find Visuals & Layout Settings
visuals_start = content.find('{/* 2. Visuals & Layout Settings */}');
accordion_start = content.find('{/* 3. Advanced Settings Accordion */}');

if visuals_start != -1 and accordion_start != -1:
    visuals_block = content[visuals_start:accordion_start]
    
    # Remove it from the original place
    content = content[:visuals_start] + content[accordion_start:]
    
    # Now insert visuals_block inside the AccordionContent
    # Find <AccordionContent className="space-y-6 pt-4 pb-2 px-1">
    acc_content_start = content.find('<AccordionContent className="space-y-6 pt-4 pb-2 px-1">')
    acc_content_tag = '<AccordionContent className="space-y-6 pt-4 pb-2 px-1">'
    
    if acc_content_start != -1:
        insert_pos = acc_content_start + len(acc_content_tag)
        
        # Modify visuals_block slightly to remove the border-t on its wrapper, as it's now inside the accordion
        visuals_block = visuals_block.replace('<div className="pt-4 border-t border-border mt-4">', '<div className="pt-2">')
        visuals_block = visuals_block.replace('<h3 className="text-lg font-medium mb-4">Design & Adjustments</h3>', '<h3 className="text-lg font-medium mb-4">Design & Adjustments</h3>')
        
        content = content[:insert_pos] + "\n" + visuals_block + "\n<div className=\"border-t border-border my-6\"></div>\n" + content[insert_pos:]

# 4. Make Accordion collapsible properly if it is not
content = content.replace('<Accordion className="w-full mt-4">', '<Accordion className="w-full mt-4" type="single" collapsible>')

with open("components/qr/qr-form.tsx", "w") as f:
    f.write(content)
